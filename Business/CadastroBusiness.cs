using ApiCadastro.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ApiCadastro.Business
{
    public class CadastroBusiness
    {

        public Cadastro Validar(Cadastro cadastro, CadastroEntities db)
        {
            if (!ValidarData(cadastro.DtNascimento))
            {
                return null;
            }   
            cadastro.Cpf = GetOnlyNumbers(cadastro.Cpf);
            if (cadastro.Cpf.Length > 11)
            {
                return null;
            }

            List<Telefone> telefones = db.Telefone.AsNoTracking().Where(t => t.CadastroId == cadastro.Id).ToList();

            foreach (Telefone item in cadastro.Telefone)
            {
                if (item.Telefone1.Length > 11)
                {
                    return null;
                }
                Telefone telefone = telefones.Where(t => t.Id == item.Id).FirstOrDefault();
                if (telefone == null)
                {
                    Telefone newTelefone = db.Telefone.Add(item);
                    db.SaveChanges();
                    telefone = newTelefone;
                }
                else
                {
                    if (telefone.Telefone1 != item.Telefone1)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                    
                }

            }
            return cadastro;

        }

        public Cadastro Inserir(Cadastro cadastro, CadastroEntities myDb)
        {
            
            Cadastro newCadastro = myDb.Cadastro.Add(cadastro);
            List<Telefone> telefones = myDb.Telefone.Where(t => t.CadastroId == cadastro.Id).ToList();

            foreach (Telefone item in cadastro.Telefone)
            {
                item.CadastroId = newCadastro.Id;
                if (item.Telefone1.Length > 11)
                {
                    return null;
                }
                Telefone telefone = telefones.Where(t => t.Id == item.Id).FirstOrDefault();
                if (telefone == null)
                {
                    Telefone newTelefone = myDb.Telefone.Add(item);
                    telefone = newTelefone;
                }
                
            }
            myDb.SaveChanges();

            return newCadastro;
        }

        public Cadastro Modificar(Cadastro cadastro, CadastroEntities myDb)
        {
            cadastro = Validar(cadastro, myDb);
            if (cadastro == null)
            {
                return null;
            }

            
            myDb.Entry(cadastro).State = EntityState.Modified;

            try
            {
                myDb.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (myDb.Cadastro.Count(e => e.Id == cadastro.Id) > 0)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return cadastro;
        }

        private string GetOnlyNumbers(string cpf)
        {
            string b = string.Empty;


            for (int i = 0; i < cpf.Length; i++)
            {
                if (Char.IsDigit(cpf[i]))
                    b += cpf[i];
            }

            return b;
        }

        private bool ValidarData(DateTime data)
        {
            if (data.Year < 2021 && data.Year > 1900)
            {
                return true;
            }
            else
            {
                return false;
            }
        }







    }
}