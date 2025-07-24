using API.Controllers;
using API.DTOs;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Application.DTOs;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Filters;

public class WellsOperationsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionName = context.MethodInfo.Name;
        var controllerName = context.MethodInfo.DeclaringType?.Name;

        if (controllerName == nameof(WellsController))
        {
            switch (actionName)
            {
                case nameof(WellsController.GetAll):
                    ConfigureGetAllWells(operation, context);
                    break;
                case nameof(WellsController.GetById):
                    ConfigureGetById(operation, context);
                    break;
                case nameof(WellsController.Create):
                    ConfigureCreateWell(operation, context);
                    break;
                case nameof(WellsController.Update):
                    ConfigureUpdateWell(operation, context);
                    break;
                case nameof(WellsController.Delete):
                    ConfigureDeleteWell(operation, context);
                    break;
            }
        }
        else if (controllerName == nameof(FileController))
        {
            switch (actionName)
            {
                case nameof(FileController.ImportFromExcel):
                    ConfigureImportFromExcel(operation, context);
                    break;
                case nameof(FileController.ExportToExcel):
                    ConfigureExportToExcel(operation, context);
                    break;
            }
        }
    }

    private void ConfigureGetAllWells(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Clear();
        operation.Responses.Add("200", new OpenApiResponse
        {
            Description = "OK",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<WellDto>), context.SchemaRepository)
                }
            }
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal Server Error"
        });
    }
    
    private void ConfigureGetById(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters[0].Required = true;
        
        operation.Responses.Clear();
        operation.Responses.Add("200", new OpenApiResponse
        {
            Description = "OK",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(WellDto), context.SchemaRepository)
                }
            }
        });
        operation.Responses.Add("404", new OpenApiResponse
        {
            Description = "Скважина не найдена"
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal server error"
        });
    }
    
    private void ConfigureCreateWell(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.RequestBody = new OpenApiRequestBody
        {
            Description = "Payload для создании скважины",
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(CreateWellRequest), context.SchemaRepository)
                }
            }
        };
        
        operation.Responses.Clear();
        operation.Responses.Add("201", new OpenApiResponse
        {
            Description = "Well Created",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(CreateWellResponse), context.SchemaRepository)
                }
            }
        });
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Неверный ввод"
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal Server Error"
        });
    }

    private void ConfigureUpdateWell(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters[0].Required = true;

        operation.RequestBody = new OpenApiRequestBody
        {
            Description = "Payload для обновления скважины",
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(UpdateWellRequest), context.SchemaRepository)
                }
            }
        };
        
        operation.Responses.Clear();
        operation.Responses.Add("204", new OpenApiResponse
        {
            Description = "Скважина успешно обновлена"
        });
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Неверный ввод"
        });
        operation.Responses.Add("404", new OpenApiResponse
        {
            Description = "Скважина не найдена"
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal Server Error"
        });
    }

    private void ConfigureDeleteWell(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters[0].Required = true;
        
        operation.Responses.Clear();
        operation.Responses.Add("204", new OpenApiResponse
        {
            Description = "Скважина успешно удалена"
        });
        operation.Responses.Add("404", new OpenApiResponse
        {
            Description = "Скважина не найдена"
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal server error"
        });
    }

    private void ConfigureImportFromExcel(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Summary = "Импорт скважин из Excel";
        operation.Description = "Импортирует данные скважины из файла Excel";

        operation.RequestBody = new OpenApiRequestBody
        {
            Description = "Файл Excel с данными о скважине",
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["file"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                                Description = "Файл Excel для загрузки"
                            }
                        },
                        Required = new HashSet<string>() { "file" }
                    }
                }
            }
        };
        
        operation.Responses.Clear();
        operation.Responses.Add("200", new OpenApiResponse
        {
            Description = "Импорт успешно завершен",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ImportFileResponse), context.SchemaRepository)
                }
            }
        });
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Файл не предоставлен"
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal server error"
        });
    }

    private void ConfigureExportToExcel(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Summary = "Экспорт скважин в Excel";
        operation.Description = "Экспортирует все скважины в файл Excel";
        
        operation.Responses.Clear();
        operation.Responses.Add("200", new OpenApiResponse
        {
            Description = "Успешная операция - возвращает файл Excel",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary",
                    }
                }
            }
        });
        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal server error"
        });
    }
}