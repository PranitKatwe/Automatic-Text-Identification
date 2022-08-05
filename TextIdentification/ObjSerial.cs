using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextIdentification
{
    [Serializable()]	//Set this attribute to all the classes that you define to be serialized
    public class ObjSerial : ISerializable
    {

        public string TextData;

        //Default constructor
        public  ObjSerial ()
        {

            TextData = null;
        }

        //Deserialization constructor.
        public ObjSerial(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties

            TextData = (String)info.GetValue("TextData", typeof(string));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"

            info.AddValue("TextData", TextData);
        }
    }
}