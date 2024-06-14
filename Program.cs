using AvisosMonitoriaData.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// notificacoes
List<Aviso> notificacoes = [];

app.MapPost("/api", () =>
{
    notificacoes.Add(new Aviso() { Severidade = SeveridadeEnumerador.Aviso, Nome = "Aviso", Descricao = "Teste", Origem = "Origem", Spans = 1 });
    notificacoes.Add(new Aviso() { Severidade = SeveridadeEnumerador.Erro, Nome = "Erro", Descricao = "Teste", Origem = "Origem", Spans = 1 });
})
.WithName("Notificar")
.WithOpenApi();

app.MapGet("/api/{pagina}/{porPagina}", (int pagina, int porPagina) =>
{
    return new RetornoApi()
    {
        Avisos = notificacoes.Where(x => x.DataUltimaOcorrencia > DateTime.Now.AddHours(-24)).Skip(pagina * porPagina).Take(porPagina).ToList(),
        TotalPaginas = notificacoes.Count / porPagina,
        TotalRegistros = notificacoes.Count
    };
})
.WithName("Obter notificações")
.WithOpenApi();

app.Run();
