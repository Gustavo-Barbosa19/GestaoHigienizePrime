namespace GestaoHigienizePrime.ViewModels;

public class RelatorioViewModel
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Periodo { get; set; }
    public string? ClienteId { get; set; }
    public string? TipoRelatorio { get; set; }
    public List<Models.Servico> Servicos { get; set; } = new();
    public List<Models.Financeiro> Financeiros { get; set; } = new();
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}
