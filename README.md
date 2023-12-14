![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/d65e2a09-24e0-4df6-8e33-b93f37071386)


## **프로젝트 소개**

- **슈퍼마리오 메이커 모작**
- **유저가 에디터로 맵을 제작하고 온라인 플레이**
- **파이어베이스로 맵데이터를 저장하고 포톤으로 온라인 플레이 구현**
- **조장을 맡아 기획, 소스 병합, 오류 수정 및 전체적인 시스템 구현**

## 시연영상

https://youtu.be/pCr7TC-3skw

## 목차

1. **로그인 씬**
2. **로비 씬**
3. **에디터 씬**
    
    **3-1. 오브젝트 설치**
    
    **3-2. 그 밖의 에디터 기능**
    
4. **방 만들기 & 방 입장**
5. **게임 플레이**
    
    **5-1. 채팅**
    
    **5-2. 클리어**
    

### **1. 로그인 씬**

- **닉네임을 입력하고 포톤 마스터 서버 입장**
- **마스터 서버에서 바로 로비상태로 변환하게 되어있으며 로비씬으로 전환**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/351fce28-1554-423e-9221-34823d828872)


### 2**. 로비 씬**

- **로비에 있는 네트워크 인원을 표시**
- **방 리스트가 나오는 스크롤뷰와 맵 만들기, 룸 만들기 버튼**
- **방 목록은 3초에 한번 새로고침 하게 되어있으며, 방 목록이 수정된 내용이 있다면 그 즉시 새로고침**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/b23b244b-294d-4be0-9ef1-0876ffed2d1d)


### 3**. 에디터 씬**

- **마리오 플레이에 필요한 오브젝트를 설치**
- **타일맵을 3개 준비**
    - **그리드 타일맵 : 격자 무늬를 표현한 타일맵**
    - **오브젝트 타일맵 : 실제 오브젝트 타일을 설치한 타일맵**
    - **가상 타일맵 : 마우스 포지션으로 따라오는 가상 타일을 설치할 타일맵**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/7111a3bc-2c1c-41bc-98e3-37d6fc44a23a)


**3-1. 오브젝트 설치**

- **아이템이 들어가는 박스 설치**
    - **아이템 박스에는 아이템을 넣을 수 있도록 작업**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/25d6bd2a-495b-46be-a4bb-7c3c589ed140)


- **파이프 설치**
    - **파이프의 짝을 라인 렌더러로 결정**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/397a7bc2-c2de-4730-917b-899e7b94db0d)


- **게임 캐릭터 스폰 위치 지정**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/f9337674-f273-4f8e-ae96-ab1440e271d6)


- **그 밖의 오브젝트들**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/e1b4d321-515a-4f69-82ff-f88025fc27b5)


**3-2. 그 밖의 에디터 기능**

- **마우스 위치에 따른 카메라 이동**
    - **마우스로 화면을 이동하며 오브젝트 설치 기능**
- **좌클릭 설치, 우클릭 제거**
- **배경 바꾸기 기능**
- **UNDO, REDO**
    - **명령에 대한 오브젝트를 리스트에 기억해뒀다가 UNDO와 REDO**
- **모의 플레이**
    - **에디터에 설치할때 실제 오브젝트를 같이 설치하고 Play했을때 그리드를 끄고 오브젝트를 Enable**
    - **오브젝트는 포톤네트워크가 적용된 오브젝트를 Instantiate 시키며 이를 위해 에디터는 포톤 개념상 InRoom 상태**
- **전체적인 맵 형태보기**
    - **렌더텍스쳐로 전체 맵을 비추는 카메라의 이미지를 렌더**
    
   ![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/a92ab584-ae01-40a7-8da9-1dea510fd504)

    
- **맵 저장**
    - **파이어베이스에 맵데이터, 썸네일 데이터 저장**
    - **썸네일은 맵 형태보기와 같은 방식으로 렌더텍스쳐의 RawImage를 저장하고 업로드한다.**

- **맵 저장 디테일**
    - **파이어베이스 Storage 는 파일 목록에 대한 정보를 주지 않기 때문에 각 클라이언트는 어떤 맵이 업로드 되어 있는지 알 수 없음**
    - **때문에 Text파일에 직접 파일 목록을 정리하며 업로드 하는 방식을 채택**
    
    ![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/fa959f89-ac72-4677-8f3e-69ba77a41a78)

    
    - **저장 프로세스 : 파일리스트 로드 → 맵데이터 생성 → 썸네일데이터 생성 → 파일리스트에 내용 추가 → 맵데이터, 썸네일데이터, 파일리스트 파이어베이스 업로드**
    

### 4**. 방 만들기 & 방 입장**

- **방 이름과 인원 설정하고 포톤 Room Make and Join**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/366fe123-fc8d-4167-ad2e-cee70b0b9af1)


- **방 입장하면 포톤으로 동기화된 캐릭터 조작 가능**
- **방장만 맵 선택이 가능하고 선택시 RPC로 방에 있는 모든 인원들의 맵 정보가 바뀌도록 작업**
- **모두 레디하면 방장이 START 가능**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/4513805a-18c0-4cfd-939a-71716a8520c8)


![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/b8ec3b86-4d86-476e-8ab6-a7b3bed4df83)


### 5**. 게임 플레이**

**5-1. 채팅**

- **캐릭터가 상속한 캔버스로 채팅을 입력하면 RPC로 해당 클라이언트의 캐릭터 메시지를 띄움**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/4d0f4313-d76c-432b-99ec-0b9468c55e82)


**5-2 클리어**

![image](https://github.com/akffoddl5/SuperMarioMaker/assets/44525847/d133e21f-093c-492b-9bf4-a249d7debc97)


- **깃발대에 닿으면 나머지 클라이언트들은 1등한 유저로 카메라가 이동하고 포커스 인**
- **우승자 아이디가 표시되며 로비로 다시 이동**
