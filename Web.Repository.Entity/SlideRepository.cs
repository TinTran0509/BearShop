using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class SlideRepository : ISlideIRepository
    {
        readonly BearShopEntities _entities = new BearShopEntities();
        private const string KeyCache = "SlideImages";
        public void Add(Slide obj)
        {
            _entities.Slides.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.Slides.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Slide obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Slide Find(int id)
        {
            return _entities.Slides.Find(id);
        }

        public List<Slide> GetAll()
        {
            return _entities.Slides.ToList();
        }
    }
}
