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
using TrabalhoDM106.Models;

namespace TrabalhoDM106.Controllers
{
    //[Authorize]
    public class ProductsController : ApiController
    {
        private TrabalhoDM106Context db = new TrabalhoDM106Context();


        //[Authorize(Roles = "ADMIN, USER")]
        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }


        [Authorize(Roles = "ADMIN, USER")]
        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize(Roles = "ADMIN")]
        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != product.Id)
            {
                return BadRequest();
            }

            Product proMod = db.Products.AsNoTracking().FirstOrDefault(pro => pro.Id == id);

            string modifying = "";

            if (proMod.cod != product.cod) modifying = "cod";
            if (proMod.model != product.model) modifying = "model";
            if (proMod.cod != product.cod && proMod.model != product.model) modifying = "both";

            Product exist = null;

            switch(modifying)
            {
                case "cod":
                    exist = db.Products.AsNoTracking().FirstOrDefault(pro => pro.cod.Equals(product.cod));
                    
                    break;
                case "model":
                    exist = db.Products.AsNoTracking().FirstOrDefault(pro => pro.model.Equals(product.model));
                    break;
                case "both":
                    exist = db.Products.AsNoTracking().FirstOrDefault(pro => pro.cod.Equals(product.cod) || pro.model.Equals(product.model));
                    break;
                default:
                    break;
            }
            if (exist != null)
            {
                if(exist.Id != product.Id)
                {
                    return BadRequest("Nao e possivel inserir um produto com " + modifying + " semelhante a outro produto ja cadastrado!");
                }
                
            }


            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product existCodModel = db.Products.AsNoTracking().FirstOrDefault(pro => pro.cod.Equals(product.cod) || pro.model.Equals(product.model));

            if (existCodModel != null && existCodModel.Id == product.Id)
            {
                return BadRequest("Nao e possivel inserir um produto com cod e/ou modelo semelhante a outro produto ja cadastrado!");
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}