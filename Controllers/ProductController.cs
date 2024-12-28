using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Products_API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly AddDbContext _Context;

        public ProductController(AddDbContext Context)
        {
           _Context = Context;  
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProductsAsync()
        {

            //using linq

          //  return  Ok(await _Context.Products.ToListAsync());

            //using Stored procedure 

           // return Ok(await _Context.Products.FromSql($"Exec SP_GetAllProducts").ToListAsync());


            var Products =await _Context.Products.Select(p=>
            new
            {

                p.Id,
                p.Name,
                p.Price


            }).ToListAsync();   
return Ok(Products);
        }


        [HttpGet("{id}",Name ="GetProductsInfobyId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<Product>>GetProductsById(int id)
        {

            if (id < 0)
                return BadRequest("ProductID is not Valid ");



            var Product=await _Context.Products.FindAsync(id);  

            if(Product!=null) 
                return Ok(Product);
            else
                return NotFound($"Product with {id} is not Found ");  
        }

        [HttpPost(Name ="AddProducts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task  <ActionResult<Product>>AddProductsAsync(Product NewProduct )
        {

         if (NewProduct ==  null)
            return NotFound("Invalid products data ");


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);  // Return validation errors
            //}


            //  _Context.Products.Add(NewProduct);

            //using stored procedure 


            var ProductId = new SqlParameter("@ProductId", SqlDbType.Int) { Direction = ParameterDirection.Output };



            var Name = new SqlParameter("@Name", NewProduct.Name);
            var Price = new SqlParameter("@Price", NewProduct.Price);


        

           await _Context.Database.ExecuteSqlRawAsync(" EXEC SP_AddNewProducts @Name , @Price , @ProductId OUT  ", Name, Price,ProductId);

            //_Context.SaveChangesAsync(); 

            NewProduct.Id = (int)ProductId.Value;

            return CreatedAtAction("GetProductsById", new { Id = NewProduct.Id }, NewProduct);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Product>>> UpdateProductsAsync( Product UpdateProduct)
        {

            // var Product = await _Context.Products.FindAsync(UpdateProduct.Id);

            //if (Product == null)
            //    return NotFound("Product is not Found ");

            var ID = new SqlParameter("@Id", UpdateProduct.Id);
            var Name = new SqlParameter("@Name", UpdateProduct.Name);
            var Price = new SqlParameter("@Price", UpdateProduct.Price);


            await _Context.Database.ExecuteSqlRawAsync("EXEC SP_UpdateProducts @Id , @Name , @Price  ", ID,Name
                , Price);


            return Ok(UpdateProduct); 



            //  return Ok(await _Context.SaveChangesAsync());

            //var parameter = new List<SqlParameter>();
            //parameter.Add(new SqlParameter("@ProductId", UpdateProduct.ProductId));
            //parameter.Add(new SqlParameter("@Name", UpdateProduct.Name));
            //parameter.Add(new SqlParameter("@Price", UpdateProduct.Price));


            //var result = await Task.Run(() =>
            //_Context.Database.ExecuteSqlRawAsync(@"exec SP_UpdateProducts @ProductId ,  @Name ,  @Price ", parameter.ToArray()));



        }

        private static int GetResult(int result)
        {
            return result;
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Product>>> DeleteProducts(int Id)
        {

            //if (Id < 0)
            //    return BadRequest($"Products with {Id} is not valid");

            //var Product = await _Context.Products.FindAsync(Id);




            //if (Product == null)
            //    return NotFound("Product is not Found ");


            //_Context.Products.Remove(Product);

            // _Context.Products.FromSqlRaw($"Exec SP_DeleteProduct @ProductId={0}", Id);


            var ID = await _Context.Products.FindAsync(Id);

          

                if (ID == null)
                return NotFound($"Products with {Id} is not Found ");


            var ProductId = new SqlParameter("@ProductId", Id);

          
            return Ok(await _Context.Database.ExecuteSqlRawAsync("EXEC SP_DeleteProduct @ProductId  ", ProductId));

          //  ($"ProductID with ID {Id} has been Deleted Successfully :-) ");


            //  return   Ok( await _Context.SaveChangesAsync());

           // return Ok(await _Context.SaveChangesAsync());
         
        }


    }
}