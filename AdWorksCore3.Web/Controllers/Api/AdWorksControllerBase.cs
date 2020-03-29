using AdWorksCore3.Core.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AdWorksCore3.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdWorksControllerBase : ControllerBase
    {
        protected void AddHeaders(string routeName, IPagedList pagedList, QueryStringParameters parameters)
        {
            Response.Headers.Add("X-PageSize", pagedList.PageSize.ToString());
            Response.Headers.Add("X-PageCount", pagedList.PageCount.ToString());
            Response.Headers.Add("X-TotalCount", pagedList.TotalItemCount.ToString());
            Response.Headers.Add("X-ItemCount", (pagedList.LastItemOnPage - pagedList.FirstItemOnPage + 1).ToString());

            if (pagedList.HasNextPage)
            {
                string nextUrl = Url.Link(routeName,
                    new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    });
                Response.Headers.Add("X-PageNext", nextUrl);
            }
            if (pagedList.HasPreviousPage)
            {
                string prevUrl = Url.Link(routeName,
                    new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    });
                Response.Headers.Add("X-PagePrev", prevUrl);
            }
        }
    }
}
