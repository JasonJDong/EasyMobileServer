using System;
using System.Collections;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DMobile.Biz.DataAccess
{
    /// <summary>
    /// ���ݷ�������
    /// </summary>
    public abstract partial class DbUtility
    {
        #region ��̬����

        protected static readonly SortedList CacheParamList;

        #endregion ��̬����

        #region ˽�б���

        private DbProviderFactory _dbFactory;

        #endregion ˽�б���

        #region ���Զ�д

        private string _providerName;

        /// <summary>
        /// ��ȡ�����������ṩ�ߵ������ռ�����ơ�
        /// </summary>
        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                _providerName = value;
                switch (_providerName)
                {
                    case "MySql.Data.MySqlClient":
                        _dbFactory = new MySqlClientFactory();
                        break;
                    default:
                        _dbFactory = DbProviderFactories.GetFactory(_providerName);
                        break;
                }
            }
        }

        /// <summary>
        /// ��ȡ�����������ַ�����
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// ��ȡ�����û�����
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// ��ȡ�������롣
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// ��ȡ��������Դ��
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// ��ȡ���ӷ�������
        /// </summary>
        public string DataServer { get; set; }

        /// <summary>
        /// ��ȡ�����������ṩ�ߡ�
        /// </summary>
        public string Provider { get; set; }

        #endregion ���Զ�д

        #region ���캯��

        /// <summary>
        /// ��̬���캯����
        /// </summary>
        static DbUtility()
        {
            CacheParamList = SortedList.Synchronized(new SortedList());
        }

        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="providerName">connectionStrings �����ơ�</param>
        protected DbUtility(string providerName)
        {
            ProviderName = providerName;
        }

        #endregion ���캯��

        #region ��������

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        public abstract void Initialize();


        /// <summary>
        /// ������������ӵ����档
        /// </summary>
        /// <param name="cacheKey">���������Keyֵ��</param>
        /// <param name="cacheParams">Ҫ����Ĳ������顣</param>
        public void CacheParameters(string cacheKey, params DbParameter[] cacheParams)
        {
            try
            {
                CacheParamList[cacheKey] = cacheParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ������������ӵ����档
        /// </summary>
        /// <param name="cacheKey">���������Keyֵ��</param>
        /// <param name="cacheParams">Ҫ����Ĳ������顣</param>
        public void CacheParameters<TParam>(string cacheKey, params TParam[] cacheParams)
        {
            try
            {
                var dbParams = new DbParameter[cacheParams.Length];
                for (int i = 0; i < cacheParams.Length; i++)
                {
                    dbParams[i] = cacheParams[i] as DbParameter;
                }
                CacheParamList[cacheKey] = dbParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ��ȡ�����еĲ������顣
        /// </summary>
        /// <param name="cacheKey">���������Keyֵ��</param>
        /// <returns>�����еĲ������顣</returns>
        public DbParameter[] GetCachedParameters(string cacheKey)
        {
            try
            {
                var cacheParams = (DbParameter[]) CacheParamList[cacheKey];
                if (cacheParams == null)
                {
                    return null;
                }

                var cloneParams = new DbParameter[cacheParams.Length];
                for (int i = 0; i < cacheParams.Length; i++)
                {
                    cloneParams[i] = ((ICloneable) cacheParams[i]).Clone() as DbParameter;
                }

                return cloneParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ��ȡ�����еĲ������顣
        /// </summary>
        /// <param name="cacheKey">���������Keyֵ��</param>
        /// <returns>�����еĲ������顣</returns>
        public TParam[] GetCachedParameters<TParam>(string cacheKey)
        {
            try
            {
                var dbParams = (DbParameter[]) CacheParamList[cacheKey];
                if (dbParams == null)
                {
                    return null;
                }

                var cloneParams = new TParam[dbParams.Length];
                for (int i = 0; i < dbParams.Length; i++)
                {
                    cloneParams[i] = (TParam) ((ICloneable) dbParams[i]).Clone();
                }

                return cloneParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion ��������
    }
}