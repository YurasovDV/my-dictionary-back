﻿using DictionaryBack.Domain;
using DictionaryBack.Infrastructure.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.DAL.Dapper
{
    public interface IDapperFacade
    {
        Task<IEnumerable<Word>> GetAll();

        Task<IEnumerable<Word>> GetPage(WordsByTopicRequest request);
    }
}
