using System;

namespace Paralect.Machine.Transitions
{
    public interface IDataTypeRegistry
    {
        Type GetType(String typeId);
        String GetTypeId(Type type);
    }
}