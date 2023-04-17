CREATE SPATIAL INDEX spatial_index
    ON t (shape)
    USING GEOGRAPHY_AUTO_GRID;

CREATE SPATIAL INDEX spatial_index
    ON t (shape)
    USING GEOMETRY_AUTO_GRID;

CREATE SPATIAL INDEX spatial_index
    ON t (shape)
    USING GEOGRAPHY_AUTO_GRID
    WITH  (
            DATA_COMPRESSION = PAGE
          );