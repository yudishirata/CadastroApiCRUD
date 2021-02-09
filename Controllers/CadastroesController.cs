using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCadastro.Models;
using System.Web.Http.Cors;
using ApiCadastro.Business;

namespace ApiCadastro.Controllers
{
    public class CadastroesController : ApiController
    {
        private CadastroEntities db = new CadastroEntities();
        private CadastroBusiness cadastroBusiness = new CadastroBusiness();
        

        // GET: api/Cadastroes

        // Allow CORS for all origins. (Caution!)
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Cadastro> GetCadastro()
        {
            var teste = db.Cadastro.ToList();
            return teste;
        }

        // GET: api/Cadastroes/5
        [ResponseType(typeof(Cadastro))]

        // Allow CORS for all origins. (Caution!)
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetCadastro(int id)
        {
            Cadastro cadastro = db.Cadastro.Find(id);
            if (cadastro == null)
            {
                return NotFound();
            }

            return Ok(cadastro);
        }

        // PUT: api/Cadastroes/5
        [ResponseType(typeof(void))]

        // Allow CORS for all origins. (Caution!)
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutCadastro(int id, Cadastro cadastro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cadastro.Id)
            {
                return BadRequest();
            }

            Cadastro updatedCadastro = cadastroBusiness.Modificar(cadastro, db);

            if (updatedCadastro == null)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cadastroes
        [ResponseType(typeof(Cadastro))]

        // Allow CORS for all origins. (Caution!)
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostCadastro(Cadastro cadastro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           cadastro = cadastroBusiness.Inserir(cadastro, db);

            if (cadastro == null)
            {
                return BadRequest();
            }


            return CreatedAtRoute("DefaultApi", new { id = cadastro.Id }, cadastro);
        }

        // DELETE: api/Cadastroes/5
        [ResponseType(typeof(Cadastro))]

        // Allow CORS for all origins. (Caution!)
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteCadastro(int id)
        {
            Cadastro cadastro = db.Cadastro.Find(id);
            if (cadastro == null)
            {
                return NotFound();
            }
            var telefones = new List<Telefone>();
            telefones = cadastro.Telefone.ToList();
            foreach (Telefone item in telefones)
            {
                db.Telefone.Remove(item);

            }
            cadastro.Telefone = null;

            db.Cadastro.Remove(cadastro);
            db.SaveChanges();

            return Ok(cadastro);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/Cadastroes/telefone/{id}")]
        public IHttpActionResult DeleteTelefone(int id)
        {
            Telefone telefone = db.Telefone.Find(id);
            if (telefone == null)
            {
                return NotFound();
            }
            db.Telefone.Remove(telefone);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CadastroExists(int id)
        {
            return db.Cadastro.Count(e => e.Id == id) > 0;
        }
    }
}