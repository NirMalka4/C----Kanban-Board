using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/* 
 * DTO represents a record from single table.
 * It's variables represents the columns of the table.
 * When passed from the DAL to BL, they contain data retrieved from DB.
 * When passed from BL to DAL, they contain data that should be written to DB.
 * Each DTO hold a reference to DAO object which responsible to manipulate the DTO table.
 */

namespace IntroSE.Kanban.Backend.DataLayer.DTOs
{
    interface DTO
    {
        ///<summary> 
        ///Add new row to the DB with the attributes of this instance. 
        ///</summary>
        void Insert();

        ///<summary> 
        ///Delete the row from the DB representing this instacne. 
        ///</summary>
        void Delete();

        ///<summary> 
        /// Return all rows from this instance compatibale table.
        ///</summary>
        IList<DTO> Select();

        ///<summary> 
        ///Remvoe all rows from this compatibale table.
        ///</summary>
        void Clear();
    }
}



