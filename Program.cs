using Microsoft.EntityFrameworkCore;
using NotasApi.Data;
using NotasApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.MapPost("/notas", async (Nota nota, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(nota.Aluno) || nota.Aluno.Length > 100)
        return Results.BadRequest("O nome do aluno é obrigatório e deve ter no máximo 100 caracteres.");

    if (string.IsNullOrWhiteSpace(nota.Disciplina) || nota.Disciplina.Length > 100)
        return Results.BadRequest("A disciplina é obrigatória e deve ter no máximo 100 caracteres.");

    if (nota.Valor < 0 || nota.Valor > 10)
        return Results.BadRequest("O valor da nota deve estar entre 0 e 10.");

    nota.DataLancamento = DateTime.UtcNow;
    db.Notas.Add(nota);
    await db.SaveChangesAsync();
    return Results.Created($"/notas/{nota.Id}", nota);
});

app.MapGet("/notas", async (AppDbContext db) => await db.Notas.ToListAsync());

app.MapGet("/notas/{id}", async (Guid id, AppDbContext db) =>
    await db.Notas.FindAsync(id) is Nota nota ? Results.Ok(nota) : Results.NotFound());

app.MapPut("/notas/{id}", async (Guid id, Nota input, AppDbContext db) =>
{
    var nota = await db.Notas.FindAsync(id);
    if (nota is null) return Results.NotFound();

    nota.Aluno = input.Aluno;
    nota.Disciplina = input.Disciplina;
    nota.Valor = input.Valor;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/notas/{id}", async (Guid id, AppDbContext db) =>
{
    var nota = await db.Notas.FindAsync(id);
    if (nota is null) return Results.NotFound();

    db.Notas.Remove(nota);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
