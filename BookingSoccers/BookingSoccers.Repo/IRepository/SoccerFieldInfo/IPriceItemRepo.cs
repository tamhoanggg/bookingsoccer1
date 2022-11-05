﻿using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface IPriceItemRepo : IBaseRepository<PriceItem>
    {
        Task<PriceItem> getFieldViaPriceItem(int Id);

        Task<PriceItem> getAPriceItemDetail(int id);    
    }
}
