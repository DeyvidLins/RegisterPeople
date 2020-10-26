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
    public class PessoasController : Controller
    {

        private readonly Contexto _contexto;

        public PessoasController(Contexto contexto)
        {
            _contexto = contexto;
        }


        public ViewResult Index(string sortOrder, string searchString)//Página principal 
        {
            //Filtragem por nome
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var pessoas = from p in _contexto.Pessoas
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                pessoas = pessoas.Where(p =>p.nome.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    pessoas = pessoas.OrderByDescending(p => p.nome);
                    break;
                case "Date":
                    pessoas = pessoas.OrderBy(p => p.dataNascimento);
                    break;
                case "date_desc":
                    pessoas = pessoas.OrderByDescending(p => p.salario);
                    break;
                default:
                    pessoas = pessoas.OrderBy(p => p.nome);
                    break;
            }
            var tel = _contexto.Telefones.ToList();
            ViewBag.tel = tel;
            
            return View(pessoas.ToList());
        }
    
        [HttpGet]
        public IActionResult CriarPessoa()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarPessoa(Pessoa pessoa) //Realiza um create no Banco de dados
        {
            if (string.IsNullOrEmpty(pessoa.nome))// Verifica se o campo nome está vazio
                ModelState.AddModelError("nome", "Campo nome: preenchimento obrigatório.");

           
            if ((DateTime.Now.Year - pessoa.dataNascimento.Date.Year) < 18)  //Valida se a idade é menor que 18 anos
                ModelState.AddModelError("dataNascimento", "Campo Nascimento: A idade deve ser maior que 18 anos.");

            if ((DateTime.Now.Year - pessoa.dataNascimento.Date.Year) > 60)  //Valida se a idade é  maior que 60 anos
                ModelState.AddModelError("dataNascimento", "Campo Nascimento: A idade não pode ser maior que 60 anos.");

            if (ModelState.IsValid)
            {

                _contexto.Add(pessoa);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return View(pessoa);
        }
        [HttpGet]
        public IActionResult AtualizarPessoa(int? id)
        {
            if (id != null)
            {
                Pessoa pessoa = _contexto.Pessoas.Find(id);
                return View(pessoa);
            }
            else return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> AtualizarPessoa(int? id, Pessoa pessoa)//Atualiza
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    _contexto.Update(pessoa);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else return View(pessoa);
            }

            else return View(pessoa);
        }

        [HttpGet]
        public IActionResult ExcluirPessoa(int? id)
        {
            if (id != null)
            {
                Pessoa pessoa = _contexto.Pessoas.Find(id);
                return View(pessoa);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> ExcluirPessoa(int? id, Pessoa pessoa)//Exclui
        {
            if (id != null)
            {
                _contexto.Remove(pessoa);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else return NotFound();
        }
      
        [HttpGet]
        public IActionResult DetalhesPessoa(int? id)//Detalhes
        {
            Pessoa pessoa = _contexto.Pessoas.Find(id);

            if (pessoa == null)
                return NotFound();
            else
                return View(pessoa);
            
        }
    
    }
}

