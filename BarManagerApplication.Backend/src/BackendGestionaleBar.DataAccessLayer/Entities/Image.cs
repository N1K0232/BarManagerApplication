﻿using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Image : BaseEntity
{
    public string Path { get; set; }

    public long Length { get; set; }
}