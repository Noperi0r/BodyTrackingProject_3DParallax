# BodyTrackingProject_3DParallax

A **Motion Parallax (Window Metaphor)** implementation using Azure Kinect and Unity. Provides 3D spatial perception through real-time user viewpoint tracking.

> Azure Kinect와 Unity를 활용한 **Motion Parallax (Window Metaphor)** 구현 프로젝트입니다. 사용자의 시점을 실시간 추적하여 디스플레이를 통해 3차원 공간감을 제공합니다.

## Environment
- **Unity Editor**: 2022.3.8f1
- **Azure Kinect SDK**: 1.4.1
- **Azure Kinect Body Tracking**: 1.1.0

## ⚠️ Setup

Copy `directml.dll` from Unity project and paste it into the Unity Editor installation path:

> `directml.dll` 파일을 Unity 프로젝트에서 복사하여 Unity Editor 설치 경로에 붙여넣기:
```
C:\Program Files\Unity\Hub\Editor\2022.3.8f1\Editor
```

---

## Technical Overview

### 1. Head Tracking

Tracks user's eye position in real-time via Azure Kinect Body Tracking SDK.

> Azure Kinect Body Tracking SDK를 통해 사용자의 눈 위치를 실시간으로 추적합니다.
```csharp
// Eye position calculation
Vector3 eyePosition = (leftEye + rightEye) / 2;

// findClosestTrackerBody(): Recognizes only the closest single user
// 가장 가까운 단일 사용자만 인식

// InputPositionEye(): Returns eye position as 3D Vector
// 3D Vector 형태로 눈 위치 반환
```

### 2. Coordinate System Calibration

Transforms Kinect coordinate system (sensor center = origin) to TV coordinate system (screen center = origin).

> Kinect 좌표계(센서 중심 = 원점)를 TV 좌표계(화면 중심 = 원점)로 변환합니다.
```csharp
// Calibration: Kinect origin → Screen center
// Kinect 원점을 화면 중심으로 보정
Vector3 calibratedPosition = trackedPosition + new Vector3(0, screenHeight / 2, 0);
```

### 3. Screen Space Transformation

Converts user position to relative coordinates based on actual screen size.

> 실제 화면 크기를 기준으로 사용자 위치를 상대 좌표로 변환합니다.
```csharp
// Screen size calculation (inch → meter)
// 화면 크기 계산 (inch → meter)
float diagonalMeter = diagonalInch * 0.0254f;
float screenWidth = diagonalMeter * Mathf.Sin(Mathf.Atan(aspectRatio));
float screenHeight = diagonalMeter * Mathf.Cos(Mathf.Atan(aspectRatio));

// Screen space transform
// Screen Space 변환
Vector3 screenSpaceCoord = realPosition / screenWidth;
```

### 4. Virtual Camera Frustum Distortion

Moves virtual camera to match user viewpoint and distorts frustum to create Motion Parallax effect.

> 사용자 시점에 맞춰 가상 카메라를 이동시키고, Frustum을 왜곡하여 Motion Parallax 효과를 생성합니다.
```csharp
void LateUpdate() {
    // Translation vector: move camera to user's eye position
    // 변환 벡터: 카메라를 사용자 눈 위치로 이동
    Vector3 tVector = eyeScreenSpace - cameraScreenSpace;
    
    // Apply translation to camera
    // 카메라에 변환 적용
    virtualCamera.transform.position += tVector;
    
    // Distort frustum to maintain screen boundaries
    // 화면 경계 유지를 위해 Frustum 왜곡
    frustum.left -= tVector.x;
    frustum.right -= tVector.x;
    frustum.top -= tVector.y;
    frustum.bottom -= tVector.y;
}
```

### 5. Input Filtering

Applies Digital Filters to remove noise from Kinect tracking data.

> Kinect 트래킹 데이터의 노이즈를 제거하기 위해 Digital Filter를 적용합니다.
```csharp
public enum FilterType { Average, Exponential }

// Average Filter: Stable, slow response
// Average Filter: 안정적, 변화 반응 느림
Vector3 AverageFilter(Vector3[] samples) {
    return samples.Average();
}

// Exponential Filter: Fast response, noise sensitive  
// Exponential Filter: 빠른 반응, 노이즈에 민감
Vector3 ExponentialFilter(Vector3 current, Vector3 previous, float alpha) {
    return alpha * current + (1 - alpha) * previous;
}
```

---

## Architecture
```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐
│  Azure Kinect   │────▶│   Input Filter   │────▶│ Coordinate      │
│  Body Tracking  │     │ (Avg/Exponential)│     │ Calibration     │
└─────────────────┘     └──────────────────┘     └────────┬────────┘
                                                          │
                                                          ▼
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐
│    Display      │◀────│ Frustum          │◀────│ Screen Space    │
│    (TV/Monitor) │     │ Distortion       │     │ Transform       │
└─────────────────┘     └──────────────────┘     └─────────────────┘
```

## Reference
- Rodrigues, E., et al. "Mixed Reality TVs: Applying Motion Parallax for Enhanced Viewing and Control Experiences on Consumer TVs." *SVR 2017*
