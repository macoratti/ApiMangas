using ApiMangas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMangas.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FileUploadController> logger;

        public FileUploadController(IWebHostEnvironment env,
            ILogger<FileUploadController> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
                                 [FromForm] IEnumerable<IFormFile> files)
        {
            var numeroMaximoArquivos = 3;
            long tamanhoMaximoArquivo = 1024 * 100;//102400 bytes ou 100KB
            var arquivoProcessados = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> resultadoUploads = new();

            foreach (var _arquivo in files)
            {
                if (!VerificaExtensaoArquivo(_arquivo))
                {
                    return BadRequest($"O arquivo não possui uma extensão ou não é uma imagem. " +
                        $"Extensões suportadas : .jpg/.png/.bmp");
                }

                var resultadoUpload = new UploadResult();
                resultadoUpload.FileName = _arquivo.FileName;

                if (arquivoProcessados < numeroMaximoArquivos)
                {
                    if (_arquivo.Length == 0)
                    {
                        logger.LogInformation("{FileName} tamanho é 0 (Err: 1)", _arquivo.FileName);
                        resultadoUpload.ErrorCode = 1;
                    }
                    else if (_arquivo.Length > tamanhoMaximoArquivo)
                    {
                        logger.LogInformation("{FileName} de {Length} bytes é " +
                                              "maior que o limite de {Limit} bytes (Err: 2)",
                                              _arquivo.FileName, _arquivo.Length, tamanhoMaximoArquivo);

                        resultadoUpload.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            var path = Path.Combine(env.WebRootPath, "images", _arquivo.FileName);
                            await using FileStream fs = new(path, FileMode.Create);
                            await _arquivo.CopyToAsync(fs);

                            logger.LogInformation("{FileName} salvo em {Path}",
                                                  _arquivo.FileName, path);

                            resultadoUpload.Uploaded = true;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} erro ao enviar (Err: 3): {Message}",
                                              _arquivo.FileName, ex.Message);
                            resultadoUpload.ErrorCode = 3;
                        }
                    }
                    arquivoProcessados++;
                }
                else
                {
                    logger.LogInformation("{FileName} não enviado pois o " +
                               "request excedeu {Count} files (Err: 4)",
                               _arquivo.FileName, numeroMaximoArquivos);
                    resultadoUpload.ErrorCode = 4;
                }
                resultadoUploads.Add(resultadoUpload);
            }
            return new CreatedResult(resourcePath, resultadoUploads);
        }

        private bool VerificaExtensaoArquivo(IFormFile file)
        {
            string[] extensoes = new string[] { "jpg", "bmp", "png" };

            var nomeArquivoExtensao = file.FileName.Split(".")[1];

            if (string.IsNullOrEmpty(nomeArquivoExtensao) ||
                !extensoes.Contains(nomeArquivoExtensao))
            {
                return false;
            }

            return true;
        }
    }
}
