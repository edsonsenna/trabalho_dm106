using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TrabalhoDM106.br.com.correios.ws;
using TrabalhoDM106.CRMClient;
using TrabalhoDM106.Models;

namespace TrabalhoDM106.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private TrabalhoDM106Context db = new TrabalhoDM106Context();

        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        public IQueryable<Order> GetOrders()
        {
            return db.Orders.Include("OrderItems");
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.customerEmail.Equals(User.Identity.Name) || User.IsInRole("ADMIN"))
            {
                return Ok(order);
            }

            return Unauthorized();
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        [Authorize]
        public IHttpActionResult PostOrder(Order order)
        {
            double weight = 0;
            double price = 0;

            order.orderItems.ForEach(delegate (OrderItem item) 
            {
                Product product = db.Products.Find(item.ProductId);
                weight += product.weight;
                price += item.qtd * product.price;
            });

            order.deliverDate = null;
            order.deliverPrice = 0;
            order.status = "aberto";
            order.totalPrice = price;
            order.totalWeight = weight;
            order.orderDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        [Authorize]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.customerEmail.Equals(User.Identity.Name) || User.IsInRole("ADMIN"))
            {
                db.Orders.Remove(order);
                db.SaveChanges();

                return Ok(order);
            }

            return Unauthorized();
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
        [ResponseType(typeof(Order))]
        [HttpGet]
        [Route("ordersByEmail")]
        public IHttpActionResult OrdersByCustomerEmail(string email)
        {
            if(User.Identity.Name.Equals(email) || User.IsInRole("ADMIN"))
            {   
                IQueryable<Order> orders = db.Orders.Where(ord => ord.customerEmail == email);

                return Ok(orders);
            }


            return Unauthorized();
        }

        [ResponseType(typeof(Order))]
        [HttpGet]
        [Route("close")]
        public IHttpActionResult CloseOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (User.Identity.Name.Equals(order.customerEmail) || User.IsInRole("ADMIN"))
            {
                if(order.deliverPrice == 0)
                {
                    order.status = "fechado";

                    db.Entry(order).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OrderExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return Ok(order);

                }
            }

            return Unauthorized();
        }

  
        public string SearchCep(string email)
        {
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(email);
            if (customer != null)
            {
                return customer.zip;
            }
            else
            {
                return null;
            }
        }


        [ResponseType(typeof(Order))]
        [Authorize]
        [HttpGet]
        [Route("frete")]
        public IHttpActionResult CalculaFrete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            if (User.Identity.Name.Equals(order.customerEmail) || User.IsInRole("ADMIN"))
            {
                double width = 0;
                double height = 0;
                double weight = 0;
                double length = 0;

                order.orderItems.ForEach(delegate (OrderItem item)
                {
                    length += item.Product.length;
                    weight += item.Product.weight;
                    height = item.Product.height > height ? item.Product.height : height;
                    width = item.Product.width > width ? item.Product.width : width;
                });

                string cep = this.SearchCep(order.customerEmail);

                if(cep != null)
                {
                    string frete;
                    CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();
                    cResultado resultado = correios.CalcPrecoPrazo("", "", "40010", "35600000", cep, weight.ToString(), 1, Convert.ToDecimal(length), Convert.ToDecimal(height), Convert.ToDecimal(width), 30, "N", 100, "S");
                    if (resultado.Servicos[0].Erro.Equals("0"))
                    {
                        frete = "Valor do frete: " + resultado.Servicos[0].Valor + " - Prazo de	entrega: " + resultado.Servicos[0].PrazoEntrega + "	dia(s)";
                        return Ok(frete);
                    }
                    else
                    {
                        return BadRequest("Código	do	erro:	" + resultado.Servicos[0].Erro + "-" + resultado.Servicos[0].MsgErro);
                    }

                    
                }

                return NotFound();
               
            }


            return Unauthorized();

            
        }
    }
}