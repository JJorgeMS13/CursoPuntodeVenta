using LinqToDB;
using Punto_de_ventas.Conexion;
using Punto_de_ventas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Punto_de_ventas.ModelClass
{
    public class Departamento : Coneccion
    {
        public bool insertDptoCat(string dptocat, int idDpto, string type)
        {
            bool valor = false;
            if (type == "dpto")
            {
                var departamento = Departamento.Where(d => d.Departamento == dptocat).ToList();
                if (0 == departamento.Count)
                {
                    Departamento.Value(d => d.Departamento, dptocat).Insert();
                    valor = true;
                }

            }
            else
            {
                var categoria = Categoria.Where(c => c.Categoria == dptocat).ToList();
                if (0 == categoria.Count)
                {
                    Categoria.Value(c => c.Categoria, dptocat)
                        .Value(c => c.IdDpto, idDpto)
                        .Insert();
                    valor = true;
                }
            }
            return valor;
        }
        public void searchDpto(DataGridView dataGridview, string campo, int idDpto, int funcion)
        {
            switch (funcion)
            {
                case 1:
                    IEnumerable<Departamentos> query;
                    if (campo == "")
                    {
                        query = Departamento.ToList();
                    }
                    else
                    {
                        query = Departamento.Where(d => d.Departamento.StartsWith(campo));
                    }
                    dataGridview.DataSource = query.ToList();
                    dataGridview.Columns[0].Visible = false;
                    break;
                case 2:
                    IEnumerable<Categorias> query2;
                    query2 = Categoria.Where(c => c.IdDpto == idDpto).ToList();
                    dataGridview.DataSource = query2.ToList();
                    dataGridview.Columns[0].Visible = false;
                    dataGridview.Columns[2].Visible = false;
                    break;
                default:
                    break;
            }
        }
        public bool updateDptoCat(string dptocat, int idDpto, int idcat, string type)
        {
            bool valor = false;
            if (type == "dpto")
            {

                var departamento = Departamento.Where(d => d.Departamento == dptocat).ToList();
                if (0 == departamento.Count  || idDpto == departamento[0].IdDpto )
                {
                    Departamento.Where(d => d.IdDpto == idDpto)
                        .Set(d => d.Departamento, dptocat)
                        .Update();
                    valor = true;
                }
            }
            else
            {
                var categoria = Categoria.Where(c => c.Categoria == dptocat).ToList();
                if (0 == categoria.Count || idcat == categoria[0].IdCat)
                {
                    Categoria.Where(c => c.IdCat == idcat)
                        .Set(c => c.Categoria, dptocat)
                        .Set(c => c.IdDpto, idDpto)
                        .Update();
                    valor = true;
                }
            }
            return valor;
        }

        public void deleteDptoCat(int idDpto, int idCat, string type)
        {
            if (type == "dpto")
            {
                Departamento.Where(d => d.IdDpto == idDpto).Delete();
                Categoria.Where(c => c.IdDpto == idDpto).Delete();
            }
            else
            {
                Categoria.Where(c => c.IdCat == idCat).Delete();
            }
        }
    }
}