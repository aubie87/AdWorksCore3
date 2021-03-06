﻿using AdWorksCore3.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

            // need to add filter parameters

            if (pagedList.HasNextPage)
            {
                string nextUrl = Url.Link(routeName, parameters.GetNextPageQueryString());
                Response.Headers.Add("X-PageNext", nextUrl);
            }
            if (pagedList.HasPreviousPage)
            {
                string prevUrl = Url.Link(routeName, parameters.GetPrevPageQueryString());
                Response.Headers.Add("X-PagePrev", prevUrl);
            }
        }
    }
}
