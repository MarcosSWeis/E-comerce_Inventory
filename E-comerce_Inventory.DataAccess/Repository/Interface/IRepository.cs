using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository.Interface
{
    //El respositorio es generico , sol ole indio que T debe ser una clase , entonce a el repositorio le podemos enviar la store , el product, category etc
    public interface IRepository<T> where T : class
    {
        //1°) Usamos la expresion para indicarle todos los filtros que vamso a tener, por medio de un delegado,es igual a null en caso de que no use filtro
        //2°) Esta funcion que es un IQueryable generico es para el ordenamiento de los datos, es igual a null en caso de que no se necesite ordenar la lista
        //3°) En caso de que deseemos agregar mas propiedades
        public IEnumerable<T> GetAll(Expression<Func<T,bool>> filter = null,Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,string addProperties = null);

        public T GetById(int id);

        //no uso ordenamiento ya que es un solo elemento el que retorno
        T GetFirst(Expression<Func<T,bool>> filter = null,string addProperties = null);

        void Add(T entity);

        //T Update(T entity);

        void Delete(int id);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
