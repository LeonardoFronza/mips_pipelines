public class Instrucao
{
    public string Label { get; set; }
    public string Opcode { get; set; }
    public string Op1 { get; set; }
    public string Op2 { get; set; }
    public string Op3 { get; set; }
    public int Temp1 { get; set; }
    public int Temp2 { get; set; }
    public int Temp3 { get; set; }
    public bool Valida { get; set; }

    public Instrucao(string label, string opcode, string op1, string op2, string op3, int temp1, int temp2, int temp3, bool valida)
    {
        this.Label = label;
        this.Opcode = opcode;
        this.Op1 = op1;
        this.Op2 = op2;
        this.Op3 = op3;
        this.Temp1 = temp1;
        this.Temp2 = temp2;
        this.Temp3 = temp3;
        this.Valida = valida;
    }

    public Instrucao Clonar()
    {
        return new Instrucao(this.Label, this.Opcode, this.Op1, this.Op2, this.Op3, this.Temp1, this.Temp2, this.Temp3, this.Valida);
    }
}
