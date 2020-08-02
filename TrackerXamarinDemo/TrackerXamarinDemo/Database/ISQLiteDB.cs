using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerXamarinDemo.Database
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
