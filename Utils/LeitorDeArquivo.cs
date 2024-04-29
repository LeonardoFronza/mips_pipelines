
namespace Pipelines.Utils;

public static class LeitorDeArquivo
{
    public static (List<Instrucao>, Dictionary<string, int>, Dictionary<string, int>) LerInstrucoes(string path)
    {
        List<string> opcodes = new List<string> { "ADD", "ADDI", "SUB", "SUBI", "BEQ", "J", "NOOP" };
        List<Instrucao> instrucoes = new List<Instrucao>();
        Dictionary<string, int> labels = new Dictionary<string, int>();
        Dictionary<string, int> variaveis = new Dictionary<string, int>();

        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            int j = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] partes = line.Split(' ');

                string label = "";
                string opcode = "";
                int i = 0;
                while (i < partes.Length - 1 && partes[i] == "")
                {
                    i += 1;
                }

                partes[i] = partes[i].Trim('\n').ToUpper();
                if (!opcodes.Contains(partes[i]))
                {
                    label = partes[i];
                    i += 1;
                }

                opcode = partes[i].Trim('\n').ToUpper();
                string op1 = partes.Length > i + 1 ? partes[i + 1].Trim('\n').ToUpper() : "0";
                string op2 = partes.Length > i + 2 ? partes[i + 2].Trim('\n').ToUpper() : "0";
                string op3 = partes.Length > i + 3 ? partes[i + 3].Trim('\n').ToUpper() : "0";

                Instrucao instrucao = new Instrucao(label, opcode, op1, op2, op3, temp1: 0, temp2: 0, temp3: 0, valida: false);
                instrucoes.Add(instrucao);

                if (label != "")
                {
                    labels[label] = j;

                    if (opcode == ".FILL")
                    {
                        variaveis[label] = int.Parse(op1);
                    }
                }

                j += 1;
            }
        }

        return (instrucoes, labels, variaveis);
    }
}
