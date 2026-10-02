using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopeeMVC.Models;
using System.Text.Json;
namespace ShopeeMVC.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly DbShopeeContext _context;
        public CarrinhoController(DbShopeeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            var carrinho = ObterCarrinho();
            return View(carrinho);
        }
        public async Task<IActionResult> Adicionar(int id)
        {
            var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Codigo == id);
            if (produto == null)
            {
                return NotFound();
            }
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(
            x => x.ProdutoId == produto.Codigo
            );
            if (item == null)
            {
                carrinho.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Codigo,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = 1
                });
            }
            else
            {
                item.Quantidade++;
            }
            // IMPORTANTE:
            // Salvar o carrinho na Session
            SalvarCarrinho(carrinho);

            return RedirectToAction(nameof(Index));
        }
        private List<CarrinhoItem> ObterCarrinho()
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");
            if (string.IsNullOrEmpty(carrinhoJson))
            {
                return new List<CarrinhoItem>();
            }
            return JsonSerializer.Deserialize<List<CarrinhoItem>>(carrinhoJson)
            ?? new List<CarrinhoItem>();
        }
        private void SalvarCarrinho(List<CarrinhoItem> carrinho)
        {
            var carrinhoJson = JsonSerializer.Serialize(carrinho);
            HttpContext.Session.SetString("Carrinho", carrinhoJson);
        }

        public IActionResult Remover(int id)
        {
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(
            x => x.ProdutoId == id
            );
            if (item != null)
            {
                carrinho.Remove(item);
            }
            SalvarCarrinho(carrinho);
            return RedirectToAction(nameof(Index));
        }
    }
}