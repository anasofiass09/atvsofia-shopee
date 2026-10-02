using System;
using System.Collections.Generic;

namespace ShopeeMVC.Models;

public partial class Produto
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public decimal Preco { get; set; }

    public int Estoque { get; set; }
}
