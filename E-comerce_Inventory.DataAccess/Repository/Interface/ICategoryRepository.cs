using E_comerce_Inventory.Models.DataModels;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository.Interface
{
    //deben heredar de Irepository<T> ya que esta contiene todos los metodos , y la clase que herede de ICategoryRepository
    //debe heredar de Repository<T> ya que contiene la implementacion de todos los metodos de IRepository<Category> 
    //sino  mi unidad de trabajo o cualquiera que tenga una propiedad de tipo ICategoryRepository solo tendia el metodo Update , y no el resto
    public interface ICategoryRepository :IRepository<Category>
    {
        public void Update(Category category);
    }
}
