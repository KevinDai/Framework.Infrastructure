using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure
{
    public static class Preconditions
    {
        public static void CheckNotNull(object value, string name)
        {
            CheckNotNull(value, name, string.Format("参数{0}的值不能为空", name));
        }

        public static void CheckNotNull(object value, string name, string message)
        {
            CheckNotBlank(name, "name", "名称不能为空字符串");
            CheckNotBlank(message, "message", "消息不能为空字符串");

            if (value == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        public static void CheckNotBlank(string value, string name, string message)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("名称不能为空字符串", "name");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("消息不能为空字符串", "message");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckNotBlank(string value, string name)
        {
            CheckNotBlank(value, name, string.Format("{0} must not be blank", name));
        }

        public static void CheckAny<T>(IEnumerable<T> collection, string name, string message)
        {
            CheckNotBlank(name, "name", "名称不能为空字符串");
            CheckNotBlank(message, "message", "消息不能为空字符串");

            if (collection == null || !collection.Any())
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckTrue(bool value, string name, string message)
        {
            CheckNotBlank(name, "name", "名称不能为空字符串");
            CheckNotBlank(message, "message", "消息不能为空字符串");

            if (!value)
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckFalse(bool value, string name, string message)
        {
            CheckNotBlank(name, "name", "名称不能为空字符串");
            CheckNotBlank(message, "message", "消息不能为空字符串");

            if (value)
            {
                throw new ArgumentException(message, name);
            }
        }
    }
}
