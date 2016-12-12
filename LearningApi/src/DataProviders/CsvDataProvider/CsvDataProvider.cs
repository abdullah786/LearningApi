﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningFoundation;
using System.IO;
using System.Collections;

namespace LearningFoundation.DataProviders
{
    /// <summary>
    /// DataProvider implementation in case data is comming from CSV file
    /// </summary>
    public class CsvDataProvider : IDataProvider<object[]>
    {
        IEnumerable<object[]> list = new List<object[]>();

        private StreamReader m_Reader;

        private string m_CurrentLine;

        private string m_FileName;

        private char m_Delimiter;

        private int m_SkipRows;

        public CsvDataProvider(string fileName, char delimiter, int skipRows = 0)
        {
            m_FileName = fileName;
            m_Delimiter = delimiter;
            m_SkipRows = skipRows;
            // TODO.. check if file exists.
            m_Reader = File.OpenText(m_FileName);
            for (int i = 0; i < m_SkipRows; i++)
            {
                m_CurrentLine = m_Reader.ReadLine();
            }
        }


        /// <summary>
        /// Respesent the loaded data
        /// </summary>
        public IEnumerable<object[]> DataSet
        {
            get
            {
                return list;
            }
            set
            {
                if (list != value)
                    list = value;
            }
        }

        int m_Current = 0;

        /// <summary>
        /// main constructor
        /// </summary>
        public CsvDataProvider()
        {
        }

        /// <summary>
        /// Current object of the enumerator
        /// </summary>
        public object[] Current
        {
            get
            {
                var strCols = m_CurrentLine.Split(m_Delimiter);

                //
                // Transform data from row->col in to col->row
                var rawData = new object[strCols.Length];

                // Read columns
                for (int i = 0; i < strCols.Length; i++)
                {
                    rawData[i] = strCols[i];
                }

                return rawData;               
            }
        }


        /// <summary>
        /// Current item of the enumerator
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return m_CurrentLine;
            }
        }

        /// <summary>
        /// Disposing the enumerator
        /// </summary>
        public void Dispose()
        {
            m_Reader.Dispose();
        }

        /// <summary>
        /// enumerator move one index forward
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            m_CurrentLine = m_Reader.ReadLine();
            if (m_CurrentLine != null)
                return true;
            else
                return false;
        }

        
        /// <summary>
        /// reset index of the enumerator
        /// </summary>
        public void Reset()
        {
            m_CurrentLine = null;
            m_Reader.BaseStream.Position = 0;
        }


        public object[] Run(object data, IContext ctx)
        {
            throw new MLException("IDataProvider does not support Run method.");           
        }

    }
}
