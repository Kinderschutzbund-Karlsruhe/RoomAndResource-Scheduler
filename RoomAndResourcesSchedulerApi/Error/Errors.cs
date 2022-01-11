﻿namespace RoomAndResourcesSchedulerApi.Error
{
    public enum Errors
    {
        USER_NOT_FOUND,
        USER_BLOCKED,
        USER_INVALID_PASSWORD,
        AUTHKEY_NOT_FOUND,
        AUTHKEY_INVALID,
        PERMISSIONS_FAILED,
        RESOURCE_NOT_FOUND,
        RESOURCE_DELETING_FAILED,
        RESOURCE_UPDATING_FAILED,
        EVENT_DELETING_FAILED,
        EVENT_UPDATING_FAILED,
        EVENT_INSERT_FAILED,
        EVENT_NOT_FOUND
    }
}
