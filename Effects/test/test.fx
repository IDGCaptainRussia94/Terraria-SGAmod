// Sun Concept

float4 _CellColor;  // default (1, .75, 0, 1)
float4 _EdgeColor;  // default (1, .5, 0, 1)
float4 _CellSize;   // default (.1, .1, .1, .1)
float _ScrollSpeed; // default .1
float _FadeSpeed;   // default 3
float _ColorScale;  // default .7
float _Time;		// Pass some time variable into here

float rand1D(float2 seed1, float2 seed2 = float2(39.9580123922427, 6.664016018872454)) {
    return frac(sin(dot(sin(seed1 * 139.77933944762117 + 4.106134037965656), seed2) * 262948.20341276616));
}

float2 rand2D(float2 seed1, float2 seed2 = float2(24.231035195023697, 75.60369865685902), float2 seed3 = float2(1.482036200711545, 81.67898148313031)) {
    return float2(frac(sin(dot(sin(seed1 * 188.7180835937831 + 1.668056392609527), seed2) * 23522.363916176346)),
                  frac(sin(dot(sin(seed1 * 172.60086495032573 + 2.8141864902052904), seed3) * 135794.8122457644)));
}

float3 Voronoi(float2 pos) {
    float2 centerCell = floor(pos);

    float2 closestCell;
    float2 toClosestPoint;
    float minDist = 10;
    // Find the dist to the closest point and its surrounding cell
    [unroll]
    for (int x = -1; x <= 1; x++) {
        [unroll]
        for (int y = -1; y <= 1; y++) {
            float2 cell = centerCell + float2(x, y);
            float2 toPoint = cell + rand2D(cell) - pos;
            float dist = length(toPoint);
            if (dist < minDist) {
                minDist = dist;
                closestCell = cell;
                toClosestPoint = toPoint;
            }
        }
    }

    float minEdgeDist = 10;
    // Find the distance to the closest edge
    [unroll]
    for (int x1 = -1; x1 <= 1; x1++) {
        [unroll]
        for (int y1 = -1; y1 <= 1; y1++) {
            float2 cell = centerCell + float2(x1, y1);
            float2 toPoint = cell + rand2D(cell) - pos; // Vector from pos to the cell's point

            float2 cellToClosestCell = abs(closestCell - cell);
            // If we aren't comparing the closest cell to itself
            if (cellToClosestCell.x + cellToClosestCell.y > .1) {
                float2 halfwayPoint = (toClosestPoint + toPoint) * .5;
                float2 closestCellToCellDir = normalize(toPoint - toClosestPoint);
                minEdgeDist = min(minEdgeDist, dot(halfwayPoint, closestCellToCellDir));
            }
        }
    }

    return float3(minDist, rand1D(closestCell), minEdgeDist);
}

float4 frag(float2 fragCoord : TEXCOORD0) : COLOR0 {
	float3 noise = Voronoi(float2(fragCoord.x + _Time * _ScrollSpeed, fragCoord.y) / _CellSize);
	return lerp(_EdgeColor, _CellColor, noise.z * _ColorScale * (cos(noise.y * _Time * _FadeSpeed + noise.x) * .75 + .5));
}

technique Technique1 {
    pass VoronoiTexture {
        PixelShader = compile ps_2_0 frag();
    }
}