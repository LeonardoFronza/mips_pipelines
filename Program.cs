using Pipelines;
using Pipelines.Utils;
using System;

var instrucoes = LeitorDeArquivo.LerInstrucoes("D:\\Aulas\\Pipelines\\AulaPipes.txt");
var registradores = new Dictionary<string, int>();
for (int i = 0; i < 32; i++)
    registradores[$"{i}"] = 0;

SimuladorPHT simuladorPHT = new SimuladorPHT(instrucoes.Item1, instrucoes.Item2, instrucoes.Item3, registradores);

Console.WriteLine("Você deseja executar com a predição habilitada? (s/n)");
string resposta = Console.ReadLine();

if (resposta.ToLower() == "n")
{
    simuladorPHT.DesabilitarPredicao();
}

simuladorPHT.Run();
