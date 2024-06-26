﻿using System;
using YMouseButtonControl.Core.DataAccess.Repositories;

namespace YMouseButtonControl.Core.DataAccess.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>()
        where T : class;

    void SaveChanges();
}
