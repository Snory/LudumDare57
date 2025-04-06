using System;

public class StoredTransfromEventArgs : EventArgs
{
    public TransformRecordDataGroup StoredTransformData;

    public StoredTransfromEventArgs(TransformRecordDataGroup transformDataList)
    {
        this.StoredTransformData = transformDataList;
    }
}