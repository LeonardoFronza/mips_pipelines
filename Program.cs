using Pipelines;
using Pipelines.Utils;

var instrucoes = LeitorDeArquivo.LerInstrucoes("D:\\Aulas\\Pipelines\\AulaPipes.txt");
var registradores = new Dictionary<string, int>();
for (int i = 0; i < 32; i++)
    registradores[$"{i}"] = 0;

Simulador simu = new Simulador(instrucoes.Item1, instrucoes.Item2, instrucoes.Item3, registradores);
simu.Run();