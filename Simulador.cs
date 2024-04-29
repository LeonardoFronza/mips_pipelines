
using System.Text;

namespace Pipelines;

public class Simulador
{
    private Instrucao? instrucaoWriteBack;
    private Instrucao? instrucaoMemoria;
    private Instrucao? instrucaoExecutar;
    private Instrucao? instrucaoDecodificar;
    private Instrucao? instrucaoBusca;

    private Dictionary<string, int> registradores;

    private List<Instrucao> instrucoes;
    private Dictionary<string, int> labels;
    private Dictionary<string, int> variaveis;

    private int pc;

    public Simulador(
        List<Instrucao> instrucoes,
        Dictionary<string, int> labels,
        Dictionary<string, int> variaveis,
        Dictionary<string, int> registradores)
    {
        this.instrucoes = instrucoes;
        this.labels = labels;
        this.variaveis = variaveis;
        this.pc = 0;
        this.registradores = registradores;
    }

    public void Bucar()
    {
        if (this.pc >= this.instrucoes.Count)
        {
            this.instrucaoBusca = null;
        }
        else
        {
            this.instrucaoBusca = this.instrucoes[this.pc];
        }
    }

    public void Executar()
    {
        if (this.instrucaoDecodificar == null)
        {
            this.instrucaoExecutar = null;
            return;
        }

        string opcode = this.instrucaoDecodificar.Opcode;
        this.instrucaoExecutar = this.instrucaoDecodificar.Clonar();

        switch (opcode)
        {
            case "NOOP":
                break;
            case "ADD":
            case "SUB":
                string r1 = this.instrucaoDecodificar.Op2;
                string r2 = this.instrucaoDecodificar.Op3;
                this.instrucaoExecutar.Temp1 = opcode == "ADD" ? this.registradores[r1] + this.registradores[r2] : this.registradores[r1] - this.registradores[r2];
                break;
            case "ADDI":
            case "SUBI":
                string r = this.instrucaoDecodificar.Op2;
                string imediato = this.instrucaoDecodificar.Op3;
                int valorImediato = this.variaveis[imediato];
                this.instrucaoExecutar.Temp1 = opcode == "ADDI" ? this.registradores[r] + valorImediato : this.registradores[r] - valorImediato;
                break;
            case "BEQ":
                string r0 = this.instrucaoDecodificar.Op1;
                string r3 = this.instrucaoDecodificar.Op2;
                string label = this.instrucaoDecodificar.Op3;
                if (this.registradores[r0] == this.registradores[r3])
                {
                    this.pc = this.labels[label];
                }
                break;
            case "J":
                this.pc = this.labels[this.instrucaoDecodificar.Op1];
                break;
        }
    }

    public void WriteBack()
    {
        if (this.instrucaoExecutar == null)
            return;

        switch (this.instrucaoExecutar.Opcode)
        {
            case "ADD":
            case "ADDI":
            case "SUB":
            case "SUBI":
                string r0 = this.instrucaoExecutar.Op1;
                int resultado = this.instrucaoExecutar.Temp1;
                this.registradores[r0] = resultado;
                break;
        }
    }

    public void Memoria()
    {
        this.instrucaoMemoria = this.instrucaoExecutar?.Clonar();
    }

    public void Decodificar()
    {
        if (this.instrucaoBusca == null)
        {
            this.instrucaoDecodificar = null;
            return;
        }

        this.instrucaoDecodificar = this.instrucaoBusca.Clonar();
        string opcode = this.instrucaoBusca.Opcode;

        switch (opcode)
        {
            case "ADD":
            case "SUB":
            case "ADDI":
            case "SUBI":
            case "BEQ":
                this.instrucaoDecodificar.Op1 = DecodificarNumeroRegistrador(this.instrucaoDecodificar.Op1);
                this.instrucaoDecodificar.Op2 = DecodificarNumeroRegistrador(this.instrucaoDecodificar.Op2);
                if (opcode == "ADD" || opcode == "SUB")
                {
                    this.instrucaoDecodificar.Op3 = DecodificarNumeroRegistrador(this.instrucaoDecodificar.Op3);
                }
                break;
        }
    }

    public string DecodificarNumeroRegistrador(string stringRegistrador)
    {
        return stringRegistrador.Trim().Trim('R');
    }

    public void Run()
    {
        var count = 0;
        var teste = this.instrucoes.Count;
        while (this.pc < this.instrucoes.Count)
        {
            this.WriteBack();
            this.Memoria();
            this.Executar();
            this.Decodificar();
            this.Bucar();
            this.pc += 1;

            Console.WriteLine(">>Contador: " + count++ + " " + DictionaryToString(this.registradores));
        }
    }

    public string DictionaryToString(Dictionary<string, int> registradores)
    {
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, int> entry in registradores)
        {
            sb.Append($"Registrador: {entry.Key}, Valor: {entry.Value}\n");
        }

        return sb.ToString();
    }
}