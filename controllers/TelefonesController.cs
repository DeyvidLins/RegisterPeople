using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CadastroPessoas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroPessoas.Controllers
{
    public class TelefonesController : Controller
    {
        private readonly Contexto _contexto;
        public TelefonesController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IActionResult> Index()
        {
           
            return View(await _contexto.Telefones.ToListAsync());
        }

        [HttpGet]
        public ViewResult CriarTelefone(int? id)
        {
            Telefone tel = new Telefone();

            if (id != null)
            {
                Telefone telefone = _contexto.Telefones.Find(id);
            }

            var p = _contexto.Pessoas.ToList();
            ViewBag.tel = p;
            return View(tel);
        }
        [HttpPost]
        public async Task<IActionResult> CriarTelefone(Telefone telefone) //Realiza um create no Banco de dados
        {
            if (string.IsNullOrEmpty(telefone.numero))// Verifica se o campo está vazio
                ModelState.AddModelError("numero", "Campo numero: preenchimento obrigatório.");

            if (string.IsNullOrEmpty(telefone.tipo))// Verifica se o campo está vazio
                ModelState.AddModelError("tipo", "Campo tipo: preenchimento obrigatório.");

            if (ModelState.IsValid)
            {

                _contexto.Add(telefone);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return View(telefone);
        }
        [HttpGet]
        public IActionResult AtualizarTelefone(int? id)
        {
            if (id != null)
            {
                Telefone telefone= _contexto.Telefones.Find(id);
                return View(telefone);
            }
            else return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> AtualizarTelefone(int? id, Telefone telefone)//Atualiza
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    _contexto.Update(telefone);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else return View(telefone);
            }

            else return View(telefone);
        }

        [HttpGet]
        public IActionResult ExcluirTelefone(int? id)
        {
            if (id != null)
            {
                Telefone telefone = _contexto.Telefones.Find(id);
                return View(telefone);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> ExcluirTelefone(int? id, Telefone telefone)//Exclui
        {
            if (id != null)
            {
                _contexto.Remove(telefone);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else return NotFound();
        }

        [HttpGet]
        public IActionResult DetalhesTelefone(int? id, Telefone telefone) //Detalhes

        {
            var tel = _contexto.Telefones //Faz um select no banco através do _contexto, para verificar se id_pessoa é igual ao que foi passado no parametro int? id
                  .Where(t => t.id_pessoa == (id)); 
            
            foreach (var t in tel) {

                var a = t.id;
                Telefone tele = _contexto.Telefones.Find(a);

                if (tele == null)
                    return NotFound();
                else
                    return View(tele);
            }
            ViewBag.info = "Não existi nenhum contato Telefônico dessa pessoa";
            return View(telefone);


        }
    }
}
