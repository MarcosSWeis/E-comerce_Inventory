using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace E_comerce_Inventory.DataAccess.Repository
{
    public class Repository<T> :IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        /// <summary>
        /// DbSet representa todas las entidades de ese tipo en la tabla correspondiente de la base de datos.
        /// </summary>
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //_db.Set<T>(); con esto indicamos la tabla especifica a la que queremso acceder
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = dbSet.Find(id);
            Delete(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll(Expression<Func<T,bool>> filter = null,Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,string addProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (addProperties != null)
            {
                //Busco separar las propiedades por la coma y elimina del areglo que devulve los elementos vacios
                foreach (var includePorperties in addProperties.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePorperties);

                }
                return query.ToList();
            }

            if (orderBy != null)
                return orderBy(query).ToList();

            return query.ToList();
        }


        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public T GetFirst(Expression<Func<T,bool>> filter = null,string addProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (addProperties != null)
            {
                //Busco separar las propiedades por la coma y elimina del areglo que devulve los elementos vacios
                foreach (var includePorperties in addProperties.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(includePorperties);
                }
            }

            return query.FirstOrDefault();
        }


    }
}
