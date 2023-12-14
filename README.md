# SuperMarioMaker
![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/650784f4-dab7-47dd-982e-36238b401cc5/2baebfce-a64d-45f0-a5fd-8f9304d4d7c9/Untitled.png)

## **프로젝트 소개**

- **서버-클라이언트 구조로 JavaSocket을 이용하여 온라인 배틀 지뢰찾기 구현**
- **서버를 직접 구현하여 응답대기하고 멀티 쓰레드 관리**
- **직접 만든 프로토콜로 소켓통신**
    
    ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/650784f4-dab7-47dd-982e-36238b401cc5/ac07f663-8654-4306-a427-88dc8f9aecb2/Untitled.png)
    
- **서버에 있는 데이터 베이스로 로그인 관리와 전적 관리**
- **지뢰찾기에서 사용중인 0주변 터뜨리기 알고리즘으로 BFS 사용**

## 시연영상

https://youtu.be/b8rRCJ4IDF4

https://youtu.be/Ymssx7hvPhk

## 목차

1. **서버**
    
    **1-1. 소켓 생성**
    
    **1-2. Thread 관리**
    
    **1-3. 프로토콜 통신**
    
    **1-4. 지뢰찾기 맵 생성 및 통신**
    
    **1-5. 그 밖의 통신**
    
2. **DB 작업**
    
    **2-1. 테이블**
    
    **2-2. 로그인**
    
    **2-3. 전적 관리**
    
3. **클라이언트**
    
    **3-1. 소켓 연결**
    
    **3-2. 클라이언트 Thread**
    
    **3-3. 게임플레이**
    

### **1. 서버**

**1-1. 소켓 생성**

- **서버-클라이언트 구조로 서버가 소켓을 열고 통신 대기**
- **클라이언트에서 접속하면 Thread 생성 후 리스트로 관리**
- **각 Thread 에선 명령 대기**
    - **Thread 대기 코드**
        
        ```java
        package com.java.mini;
        
        import java.awt.Color;
        import java.io.IOException;
        import java.io.ObjectInputStream;
        import java.io.ObjectOutputStream;
        import java.net.Socket;
        import java.util.ArrayList;
        import java.util.Collections;
        import java.util.Comparator;
        import java.util.List;
        
        public class GameServerThread extends Thread {
        	
        	
        	private ObjectInputStream ois;
        	private ObjectOutputStream oos;
        	private Socket client;
        	private List<GameServerThread> threadList = null;
        	private ArrayList<List<GameServerThread>> mineSweeperThreadList = null;
        	private List<GameServerThread> mineAvoiderThreadList = null;
        	private ArrayList<Integer> gameAlreadyStartList = null;
        	public static boolean minesweeperAlreadyStart = false;
        	
        	private boolean iFailMinesweep= false;
        	
        	private List<ArrayList<String>> idList = null;
        	private String myId = "";
        	
        	public GameServerThread(Socket client1 , List<GameServerThread> threadList1,ArrayList<List<GameServerThread>> mineSweeperThreadList1
        			,List<GameServerThread> mineAvoiderThreadList1,ArrayList<Integer> gameAlreadyStartList1,
        			List<ArrayList<String>> idList1) {
        		this.client = client1;
        		this.threadList=threadList1;
        		this.mineSweeperThreadList=mineSweeperThreadList1;
        		this.mineAvoiderThreadList=mineAvoiderThreadList1;
        		this.gameAlreadyStartList=gameAlreadyStartList1;
        		this.idList= idList1;
        		
        	}
        	
        	@Override
        	public synchronized void run() {
        		
        		try {
        //			br= new BufferedReader(new InputStreamReader(client.getInputStream()));
        //			bw= new BufferedWriter(new OutputStreamWriter(client.getOutputStream()));
        			ois = new ObjectInputStream(client.getInputStream());
        			oos = new ObjectOutputStream(client.getOutputStream());
        			while(true) {
        				System.out.println("커멘드 대기중..");
        				
        				Object readObject= ois.readObject();
        				ArrayList<Object> command = (ArrayList<Object>)readObject; 
        				String protocol = (String)command.get(0);
        				if(protocol.equals(ProtocolMsg.LOGIN)) {
        					System.out.println("login 요청 들어옴");
        					int canLogin = ProtocolMsg.canLogin(command);
        					
        					ArrayList<Object> response = new ArrayList<Object>();
        					response.add(ProtocolMsg.LOGIN_CHECK);
        					response.add(canLogin);
        					
        					System.out.println(ProtocolMsg.LOGIN_CHECK + canLogin);
        					
        					
        					oos.writeObject(response);
        					myId = (String)command.get(1);
        					
        				}else if(protocol.contentEquals(ProtocolMsg.JUST_LOGIN)) {
        					System.out.println("just login 요청");
        					
        					
        					String id = (String)command.get(1);
        					String pw = (String)command.get(2);
        					
        					ProtocolMsg.powerLogin(id,pw);
        					myId = (String)command.get(1);
        					
        				}else if(protocol.equals(ProtocolMsg.I_WANT_MINESWEEPER)) {
        					System.out.println("he want minesweeper");
        					if(mineSweeperThreadList.size()==0) {
        						mineSweeperThreadList.add(new ArrayList<GameServerThread>());
        						idList.add(new ArrayList<String>());
        					}
        					int roomNum = mineSweeperThreadList.size();
        					int i=0;
        					for(i=0;i<roomNum ; i++) {
        						if(mineSweeperThreadList.get(i).size()<4 && !gameAlreadyStartList.contains(i)) {
        							break;
        						}
        					}
        					
        					if(i>=roomNum) {  // 여기가 핵심  오류의 근원 >= 이냐 > 이냐
        						mineSweeperThreadList.add(new ArrayList<GameServerThread>());
        						idList.add(new ArrayList<String>());
        					}
        					
        					if(mineSweeperThreadList.get(i).size()==0) {
        						gameAlreadyStartList.remove((Integer)i);  // 이거 제대로 될지 모름. 인덱스가 지워질지도 // 널 포인터뜰지
        					}
        					if(false) { // 이미 시작한 경우는 없어야 할것이야
        //						System.out.println(gameAlreadyStartList[0] + " "+ gameAlreadyStartList[1]);
        //						System.out.println("flag1");
        //						ArrayList<Object> list = new ArrayList<Object>();
        //						list.add(ProtocolMsg.MINESWEEPER_ALREADY_START);
        //						oos.writeObject(list);
        					}else if(mineSweeperThreadList.get(i).size()==0) {
        						System.out.println("flag2");
        						
        						
        						ArrayList<Object> list = new ArrayList<>();
        						list.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
        						list.add(i);
        						oos.writeObject(list);
        						mineSweeperThreadList.get(i).add(this);
        							
        						ArrayList<Object> list2 = new ArrayList<Object>();
        						idList.get(i).add(this.myId);
        						list2.add(ProtocolMsg.SOMEONE_IN);
        						list2.add(idList.get(i).size());
        						for(String s : idList.get(i)) {
        							list2.add(s);
        						}
        //						System.out.println(idList.get(i));
        						broadcasting(mineSweeperThreadList.get(i), list2);
        						
        					}else if(mineSweeperThreadList.get(i).size()<4) {
        						System.out.println("flag3");
        						
        							ArrayList<Object> list = new ArrayList<>();
        							list.add(ProtocolMsg.YOU_CAN_MINESWEEPER);
        							list.add(i);
        							oos.writeObject(list);
        							mineSweeperThreadList.get(i).add(this);
        							
        							ArrayList<Object> list2 = new ArrayList<Object>();
        							idList.get(i).add(this.myId);
        							list2.add(ProtocolMsg.SOMEONE_IN);
        							list2.add(idList.get(i).size());
        							for(String s : idList.get(i)) {
        								list2.add(s);
        							}
        //							System.out.println(list2);
        							broadcasting(mineSweeperThreadList.get(i), list2);
        						
        						
        					}else {
        						System.out.println("flag4");
        						ArrayList<Object> list = new ArrayList<>();
        						list.add(ProtocolMsg.YOU_CANNOT_MINESWEEPER);
        						oos.writeObject(list);
        					}
        					
        					System.out.println("지뢰 찾기 방 개수 : " + mineSweeperThreadList.size());
        					System.out.print("지뢰찾기 방 인원 : ");
        					for(List<GameServerThread> p : mineSweeperThreadList ) {
        						System.out.print(p.size()+ " 명 , ");
        					}
        					System.out.println();
        					
        				}else if(protocol.equals(ProtocolMsg.I_WANTTO_AVOID_MINESWEEPER)) {
        					System.out.println("he wants to avoid minesweeper");
        					boolean masterExit =false;
        					int hisRoomNum = (int)command.get(1);
        					if(mineSweeperThreadList.get(hisRoomNum).get(0)==this) {
        						masterExit=true;
        					}
        					mineSweeperThreadList.get(hisRoomNum).remove(this);
        					idList.get(hisRoomNum).remove(myId);
        					if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
        						List<Object> list = new ArrayList<Object>();
        						list.add(ProtocolMsg.SOMEONE_EXIT);
        						list.add(idList.get(hisRoomNum).size());
        						for(String s : idList.get(hisRoomNum)) {
        							list.add(s);
        						}
        						broadcasting(mineSweeperThreadList.get(hisRoomNum), list);
        					}
        					if(masterExit && !mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
        						
        						
        							List<Object> list = new ArrayList<Object>();
        							list.add(ProtocolMsg.MINESWEEPER_MASTER_EXIT);
        							mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list);
        							
        					}
        					
        					if(mineSweeperThreadList.get(hisRoomNum).size()==0) {
        						System.out.println("이제 아무도 없음");
        						gameAlreadyStartList.remove((Integer)hisRoomNum);  // 오류 날 수 있음.
        						System.out.println("그래서 얼레디리스트에서 없앰.");
        //						List<Object> list = new ArrayList<Object>();
        //						list.add(ProtocolMsg.MINESWEEPER_GAME_END);
        //						broadcasting(threadList, list);   // 이거 이제 필요없을 듯?
        						mineSweeperThreadList.remove((Integer)hisRoomNum);
        						idList.remove((Integer)hisRoomNum);
        						System.out.println("방 비어서  없애버림 ㄷㄷ");
        					}
        					
        					System.out.println("지뢰찾기 방 개수 : "+mineSweeperThreadList.size());
        				}else if(protocol.equals(ProtocolMsg.START_MINESWEEPER)) {
        					System.out.println("they want to start Minesweeper");
        					int hisRoomNum = (int)command.get(1);
        //					minesweeperAlreadyStart = true;
        					gameAlreadyStartList.add(hisRoomNum);
        					System.out.println("방번호 : " +  hisRoomNum);
        					
        					GameServer.minesweepOverInfoList.remove(hisRoomNum);  // 이걸로 다 지워질랑가 몰라
        					GameServer.minesweepOverInfoList.put(hisRoomNum, new ArrayList<ArrayList<Object> >());
        //					GameServer.minesweepOverInfoList.get(hisRoomNum).removeAll(c)
        					List<Object> list = new ArrayList<Object>();
        					char[][] mineMap = MakeMine.makeMine();
        					list.add(ProtocolMsg.MINESWEEPER_PRESTART);
        					list.add(mineMap);
        					Color[] colors = {Color.RED, Color.YELLOW, Color.BLUE, Color.GREEN};
        					int minesweeperNum = mineSweeperThreadList.get(hisRoomNum).size();
        					System.out.println(minesweeperNum +" " + hisRoomNum);
        					for(int i=0;i<minesweeperNum;i++) {
        						mineSweeperThreadList.get(hisRoomNum).get(i).iFailMinesweep=false;
        						
        						list.add(colors[i]);
        						
        						mineSweeperThreadList.get(hisRoomNum).get(i).sendMsg(list);
        						list.remove(2);
        					}
        					
        					
        					
        				}else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {
        					System.out.println("he clicks safety zone");
        					int hisRoomNum = (int)command.get(5);
        //					int heClickRow = (int)command.get(1);
        //					int heClickCol = (int)command.get(2);
        //					Color hisColor = (Color)command.get(3);
        //					List<Object> list = new ArrayList<Object>();
        					broadcasting(mineSweeperThreadList.get(hisRoomNum), command);
        					
        					
        				}else if(protocol.equals(ProtocolMsg.USER_FINISH_MINESWEEPER)) {
        					System.out.println("user finish minesweeper");
        					
        					int hisRoomNum = (int)command.get(1);
        					
        					broadcasting(mineSweeperThreadList.get(hisRoomNum),command);
        					if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
        						List<Object> list3 = new ArrayList<>();
        						list3.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
        						list3.add(hisRoomNum);
        						mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list3);
        					}
        					
        					gameAlreadyStartList.remove((Integer)hisRoomNum);
        					
        				}else if(protocol.equals(ProtocolMsg.I_FAIL_MINESWEEP)) {
        					iFailMinesweep= true;
        					int hisRoomNum = (int)command.get(1);
        					boolean allFail = true;
        					
        					for(GameServerThread thread : mineSweeperThreadList.get(hisRoomNum)) {
        						if(!thread.iFailMinesweep) {
        							allFail=false;
        						}
        					}
        					if(allFail) {
        						List<Object> list = new ArrayList<Object>();
        						list.add(ProtocolMsg.MINESWEEP_ALL_FAIL);
        						
        						broadcasting(mineSweeperThreadList.get(hisRoomNum),list);
        						
        						if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
        							List<Object> list2 = new ArrayList<>();
        							list2.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
        							list2.add(hisRoomNum);
         							mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list2);
        						}
        						
        						gameAlreadyStartList.remove((Integer)hisRoomNum);
        					}
        					
        				}else if(protocol.equals(ProtocolMsg.MINESWEEP_OVER_INFO)) {
        					int hisRoomNum = (int)command.get(5);
        					GameServer.minesweepOverInfoList.get(hisRoomNum).add(command);
        					
        					if(GameServer.minesweepOverInfoList.get(hisRoomNum).size()==mineSweeperThreadList.get(hisRoomNum).size()) {
        						Collections.sort(GameServer.minesweepOverInfoList.get(hisRoomNum), new Comparator<ArrayList<Object> >() {
        							@Override
        							public int compare(ArrayList<Object> o1, ArrayList<Object> o2) {
        								int cnt1 = (int)o1.get(4);
        								int cnt2 = (int)o2.get(4);
        								if(cnt1<cnt2) return 1;
        								else if(cnt1>cnt2) return -1;
        								else return 1;
        								
        								
        							}
        							
        						});
        						List<Object> infolistSpecial = new ArrayList<Object>();
        						infolistSpecial.add(ProtocolMsg.MINESWEEP_OVER_ALLINFO);
        //						infolist.add(GameServer.minesweepOverInfoList);
        						System.out.println(">>"+GameServer.minesweepOverInfoList.get(hisRoomNum));
        						List<Object> infoList = new ArrayList<Object>();
        						for(ArrayList<Object> tmp1 : GameServer.minesweepOverInfoList.get(hisRoomNum)) {
        							System.out.println((String)tmp1.get(1) + tmp1.get(3) + tmp1.get(4));
        							ArrayList<Object> tmp2 = new ArrayList<Object>();
        							tmp2.add(tmp1.get(1));
        							tmp2.add(tmp1.get(3));
        							tmp2.add(tmp1.get(4));
        							infoList.add(tmp2);
        						}
        						infolistSpecial.add(infoList);
        						
        						broadcasting(mineSweeperThreadList.get(hisRoomNum), infolistSpecial);
        						
        						ProtocolMsg.battleLogInsert(infolistSpecial, "MineSweeper");
        						
        					}
        					
        					
        					
        					
        				}else if(protocol.equals(ProtocolMsg.I_WANT_RANKING)) {
        					List<Object> vNum = new ArrayList<Object>();
        					vNum.add(ProtocolMsg.VICTORY_NUM);
        					vNum.add(ProtocolMsg.getRank());
        					oos.writeObject(vNum);
        					
        				}else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
        					List<Object> logList = new ArrayList<Object>();
        					logList.add(ProtocolMsg.I_WANT_BATTLELOG);
        					logList.add(ProtocolMsg.getBattleLog());
        					oos.writeObject(logList);
        					
        				}
        				
        				System.out.println(command.toString());
        			}
        			
        			
        			
        			
        			}catch(Exception e) {
        				System.out.println("연결 하나 끊김.");
        				e.printStackTrace();
        				threadList.remove(this);
        			}
        		
        	}
        	
        	public void broadcasting(List<GameServerThread> sendList,List<Object> msgList ) {
        		
        		for(GameServerThread thread : sendList) {
        			thread.sendMsg(msgList);
        		}
        		
        	}
        	
        	public void sendMsg(List<Object> msgList) {
        		
        		try {
        			oos.writeObject(msgList);
        		} catch (IOException e) {
        			e.printStackTrace();
        		}
        		
        	}
        	
        
        }
        ```
        

**1-2. Thread 관리**

- **전체 Thread 리스트, 게임방에 들어간 Thread리스트, 게임중인 방 List 관리**
    
    ```java
    //전체 Thread List
    private List<GameServerThread> threadList = null;
    //이중 List로 방을 구분
    private ArrayList<List<GameServerThread>> mineSweeperThreadList = null;
    //게임  시작한 방번호 List
    private ArrayList<Integer> gameAlreadyStartList = new ArrayList<Integer>();
    ```
    
- **방 입장 시 아직 시작하지 않은 방을 서치하고 들어갈 방이 없으면 방 생성**

- **브로드캐스팅 함수(쓰레드 리스트에 전달할 프로토콜 메시지를 받아 통신)**

```java
public void broadcasting(List<GameServerThread> sendList,List<Object> msgList ) {
		
		for(GameServerThread thread : sendList) {
			thread.sendMsg(msgList);
		}
		
}

public void sendMsg(List<Object> msgList) {
		
		try {
			oos.writeObject(msgList);
		} catch (IOException e) {
			e.printStackTrace();
		}
		
}
```

**1-3. 프로토콜 통신**

- **프로토콜과 해당하는 데이터값을 같이 전송하여 소켓통신**
- **통신 예시(로그인)**
    - **클라이언트(로그인 시도)**
        
        ```java
        //클라이언트
        
        if(e.getSource() == enter) {
        			System.out.println("enter click");
        			String id = idField.getText();
        			String pw = pwField.getText();
        			
        			
        			String msg = ProtocolMsg.LOGIN;
        			
        //			ois.reset();
        //			oos.reset();
        			
        			boolean loginPermit = false;
        			if(id.trim().length()<4) {
        				JOptionPane.showMessageDialog(this,"아이디를 4글자 이상 입력해주세요.");
        			}else if(pw.trim().length()<4) {
        				JOptionPane.showMessageDialog(this,"비밀번호를 4글자 이상 입력해주세요.");
        			}else {
        				try {
        //					oos = new ObjectOutputStream(socket.getOutputStream());
        //					ois = new ObjectInputStream(socket.getInputStream());
        					List<Object> command = new ArrayList<>();
        					command.add(msg);
        					command.add(id);
        					command.add(pw);
        					oos.writeObject(command);
        					Thread.sleep(1300);
        					int canLogin = GameClientThread.canLogin;
        					System.out.println(canLogin);
        					if(canLogin==1) {
        						loginPermit = true;
        						JOptionPane.showMessageDialog(this,"환영합니다.");
        //						try{if(oos!=null) oos.close();}catch(IOException ioe) {}
        						myId = id;
        						myPw = pw;
        						setVisible(false);
        						GameClientThread.makeMenu.setSize(930,620);
        						GameClientThread.makeMenu.setVisible(true);
        					}else if(canLogin==2) {
        						JOptionPane.showMessageDialog(this,"비밀번호가 틀렸습니다~.");
        					}else if(canLogin==0) {
        
        						String[] str = {"예", "아니오"};
        						int select = JOptionPane.showOptionDialog(this,"등록된 아이디가 아닙니다. 이대로 접속하시겠습니까?","warning",JOptionPane.YES_NO_CANCEL_OPTION, JOptionPane.INFORMATION_MESSAGE, null,str,str[0]);
        						if(select == JOptionPane.YES_OPTION) {
        							ArrayList<Object> justLogMsg = new ArrayList<Object>();
        							justLogMsg.add(ProtocolMsg.JUST_LOGIN);
        							justLogMsg.add(id);
        							justLogMsg.add(pw);
        							
        							oos.writeObject(justLogMsg);
        							
        							
        							JOptionPane.showMessageDialog(this,"환영합니다.");
        //							try{if(oos!=null) oos.close();}catch(IOException ioe) {}
        							myId = id;
        							myPw = pw;
        							setVisible(false);
        							GameClientThread.makeMenu.setSize(1280,720);
        							GameClientThread.makeMenu.setVisible(true);
        							
        							
        						}else {
        							
        						}
        						
        //	//					try{if(ois!=null) ois.close();}catch(IOException ioe) {}
        //	//					try{if(oos!=null) oos.close();}catch(IOException ioe) {}
        					}
        					
        					
        				} catch (IOException e1) {
        					e1.printStackTrace();
        				} catch(Exception e2) {
        					e2.printStackTrace();
        				}finally {
        //					try{if(ois!=null) ois.close();}catch(IOException ioe) {}
        //					try{if(oos!=null) oos.close();}catch(IOException ioe) {}
        				}
        				
        			}
        			
        			
        			
        		}
        ```
        
    - **서버(로그인 체크 후 응답)**
        
        ```java
        if(protocol.equals(ProtocolMsg.LOGIN)) {
        		System.out.println("login 요청 들어옴");
        		int canLogin = ProtocolMsg.canLogin(command);
        		
        		ArrayList<Object> response = new ArrayList<Object>();
        		response.add(ProtocolMsg.LOGIN_CHECK);
        		response.add(canLogin);
        		
        		System.out.println(ProtocolMsg.LOGIN_CHECK + canLogin);
        		
        		
        		oos.writeObject(response);
        		myId = (String)command.get(1);
        		
        }
        ```
        
        ```java
        public static int canLogin(ArrayList<Object> command) {
        		
        		Connection conn = null;
        		Statement stmt = null;
        //		PreparedStatement pstmt = null;
        		ResultSet rs = null;
        		
        		try {
        			Class.forName("oracle.jdbc.driver.OracleDriver");
        			System.out.println("db 연결 성공");
        			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
        			String dbId = "red";
        			String dbPw = "red";
        			conn = DriverManager.getConnection(url,dbId,dbPw);
        			
        			String inputId = (String)command.get(1);
        			String inputPw = (String)command.get(2);
        			
        			String sql = "select * from login where id = '"+inputId+"'";
        			stmt= conn.createStatement();
        			rs=stmt.executeQuery(sql);
        			rs.next();
        			String getId = rs.getString("id");
        			if(getId == null) getId = "";
        			String getPw = rs.getString("pw");
        			if(getPw == null) getPw = "";
        //			rs.next();
        			if(inputId.equals(getId) && inputPw.equals(getPw)) {  
        				return 1;  // 로그인 성공
        			}else if(inputPw.equals("")) {
        				return 0;  // 로그인 정보 없음
        			}else if(!inputPw.equals(getPw)) {
        				return 2;  // 비밀번호 불일치
        			}
        			
        			
        			
        		} catch (ClassNotFoundException e) {
        			System.out.println("db 못찾음");
        		} catch(SQLException sqle) {
        			sqle.printStackTrace();
        		}finally{
        			try{if(rs!=null) rs.close();}catch(SQLException sqle) {}
        			try{if(stmt!=null) stmt.close();}catch(SQLException sqle) {}
        			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
        			
        			
        		}
        		
        		return 0;
        	}
        ```
        
    

**1-4. 지뢰찾기 맵 생성 및 통신**

- **서버에서 지뢰찾기 맵을 생성하고 요청한 방의 유저에게 맵 데이터 전달**
    
    ```java
    public static char[][] makeMine(){
    		Scanner sc= new Scanner(System.in);
    		
    		int[][] map;
    		char[][] map2;
    		int a;
    		int b;
    		int[] p1 = {0,1,0,-1,1,1,-1,-1};
    		int[] p2 = {1,0,-1,0,1,-1,-1,1};
    		
    		
    		
    		a=16;
    		b=16;
    		int n=50;
    		
    //			System.out.println("*");
    		map = new int[a][b];
    		map2 = new char[a][b];
    
    		for(int i=0;i<n;i++) {
    			
    			int row = (int)(Math.random()*a);
    			int col = (int)(Math.random()*b);
    
    			if(map[row][col]!=1 && !(row==a-1 & col==b-1) && !(row==0 & col==0)) {
    
    				map[row][col]=1;
    
    			}else {
    
    				i--;
    
    			}
    //			System.out.println(row+" " + col);
    
    		}
    		
    		
    		//지뢰 0,1 
    //		for(int i=0;i<a;i++) {
    //			for(int j=0;j<b;j++) {
    //				System.out.print(map[i][j]);
    //			}
    //			System.out.println();
    //		}
    		
    		for(int i=0;i<a;i++) {
    			for(int j=0;j<b;j++) {
    				int row = i;
    				int col = j;
    				int tmpCnt = 0;
    				for(int k=0;k<8;k++) {
    					int nextRow= row+p1[k];
    					int nextCol= col+p2[k];
    					if(nextRow>=0 && nextCol >=0 && nextRow<a && nextCol <b) {
    						if(map[nextRow][nextCol]==1) {
    							tmpCnt++;
    						}
    						
    					}
    					
    				}
    				if(map[row][col]==0)
    				map2[row][col]=(char)(tmpCnt+'0');
    				else map2[row][col]='X';
    				
    			}
    		}
    //		System.out.println("================");
    //		for(int i=0;i<a;i++) {
    //			for(int j=0;j<b;j++) {
    //				System.out.print(map2[i][j]);
    //			}
    //			System.out.println();
    //		}
    //		
    		return map2;
    	
    }
    ```
    
- **유저가 버튼 클릭시 서버에서 같은 방에 있는 사람에게 해당 내용 브로드캐스팅**

```java
}else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {
					System.out.println("he clicks safety zone");
					int hisRoomNum = (int)command.get(5);
					broadcasting(mineSweeperThreadList.get(hisRoomNum), command);
}
```

**1-5. 그 밖의 통신**

- **서버 Thread에서 처리하는 명령들**
    
    ```java
    @Override
    	public synchronized void run() {
    		
    		try {
    //			br= new BufferedReader(new InputStreamReader(client.getInputStream()));
    //			bw= new BufferedWriter(new OutputStreamWriter(client.getOutputStream()));
    			ois = new ObjectInputStream(client.getInputStream());
    			oos = new ObjectOutputStream(client.getOutputStream());
    			while(true) {
    				System.out.println("커멘드 대기중..");
    				
    				Object readObject= ois.readObject();
    				ArrayList<Object> command = (ArrayList<Object>)readObject; 
    				String protocol = (String)command.get(0);
    				if(protocol.equals(ProtocolMsg.LOGIN)) {
    					System.out.println("login 요청 들어옴");
    					int canLogin = ProtocolMsg.canLogin(command);
    					
    					ArrayList<Object> response = new ArrayList<Object>();
    					response.add(ProtocolMsg.LOGIN_CHECK);
    					response.add(canLogin);
    					
    					System.out.println(ProtocolMsg.LOGIN_CHECK + canLogin);
    					
    					
    					oos.writeObject(response);
    					myId = (String)command.get(1);
    					
    				}else if(protocol.contentEquals(ProtocolMsg.JUST_LOGIN)) {
    					System.out.println("just login 요청");
    					
    					
    					String id = (String)command.get(1);
    					String pw = (String)command.get(2);
    					
    					ProtocolMsg.powerLogin(id,pw);
    					myId = (String)command.get(1);
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANT_MINESWEEPER)) {
    					System.out.println("he want minesweeper");
    					if(mineSweeperThreadList.size()==0) {
    						mineSweeperThreadList.add(new ArrayList<GameServerThread>());
    						idList.add(new ArrayList<String>());
    					}
    					int roomNum = mineSweeperThreadList.size();
    					int i=0;
    					for(i=0;i<roomNum ; i++) {
    						if(mineSweeperThreadList.get(i).size()<4 && !gameAlreadyStartList.contains(i)) {
    							break;
    						}
    					}
    					
    					if(i>=roomNum) {  // 여기가 핵심  오류의 근원 >= 이냐 > 이냐
    						mineSweeperThreadList.add(new ArrayList<GameServerThread>());
    						idList.add(new ArrayList<String>());
    					}
    					
    					if(mineSweeperThreadList.get(i).size()==0) {
    						gameAlreadyStartList.remove((Integer)i);  // 이거 제대로 될지 모름. 인덱스가 지워질지도 // 널 포인터뜰지
    					}
    					if(false) { // 이미 시작한 경우는 없어야 할것이야
    //						System.out.println(gameAlreadyStartList[0] + " "+ gameAlreadyStartList[1]);
    //						System.out.println("flag1");
    //						ArrayList<Object> list = new ArrayList<Object>();
    //						list.add(ProtocolMsg.MINESWEEPER_ALREADY_START);
    //						oos.writeObject(list);
    					}else if(mineSweeperThreadList.get(i).size()==0) {
    						System.out.println("flag2");
    						
    						
    						ArrayList<Object> list = new ArrayList<>();
    						list.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
    						list.add(i);
    						oos.writeObject(list);
    						mineSweeperThreadList.get(i).add(this);
    							
    						ArrayList<Object> list2 = new ArrayList<Object>();
    						idList.get(i).add(this.myId);
    						list2.add(ProtocolMsg.SOMEONE_IN);
    						list2.add(idList.get(i).size());
    						for(String s : idList.get(i)) {
    							list2.add(s);
    						}
    //						System.out.println(idList.get(i));
    						broadcasting(mineSweeperThreadList.get(i), list2);
    						
    					}else if(mineSweeperThreadList.get(i).size()<4) {
    						System.out.println("flag3");
    						
    							ArrayList<Object> list = new ArrayList<>();
    							list.add(ProtocolMsg.YOU_CAN_MINESWEEPER);
    							list.add(i);
    							oos.writeObject(list);
    							mineSweeperThreadList.get(i).add(this);
    							
    							ArrayList<Object> list2 = new ArrayList<Object>();
    							idList.get(i).add(this.myId);
    							list2.add(ProtocolMsg.SOMEONE_IN);
    							list2.add(idList.get(i).size());
    							for(String s : idList.get(i)) {
    								list2.add(s);
    							}
    //							System.out.println(list2);
    							broadcasting(mineSweeperThreadList.get(i), list2);
    						
    						
    					}else {
    						System.out.println("flag4");
    						ArrayList<Object> list = new ArrayList<>();
    						list.add(ProtocolMsg.YOU_CANNOT_MINESWEEPER);
    						oos.writeObject(list);
    					}
    					
    					System.out.println("지뢰 찾기 방 개수 : " + mineSweeperThreadList.size());
    					System.out.print("지뢰찾기 방 인원 : ");
    					for(List<GameServerThread> p : mineSweeperThreadList ) {
    						System.out.print(p.size()+ " 명 , ");
    					}
    					System.out.println();
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANTTO_AVOID_MINESWEEPER)) {
    					System.out.println("he wants to avoid minesweeper");
    					boolean masterExit =false;
    					int hisRoomNum = (int)command.get(1);
    					if(mineSweeperThreadList.get(hisRoomNum).get(0)==this) {
    						masterExit=true;
    					}
    					mineSweeperThreadList.get(hisRoomNum).remove(this);
    					idList.get(hisRoomNum).remove(myId);
    					if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
    						List<Object> list = new ArrayList<Object>();
    						list.add(ProtocolMsg.SOMEONE_EXIT);
    						list.add(idList.get(hisRoomNum).size());
    						for(String s : idList.get(hisRoomNum)) {
    							list.add(s);
    						}
    						broadcasting(mineSweeperThreadList.get(hisRoomNum), list);
    					}
    					if(masterExit && !mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
    						
    						
    							List<Object> list = new ArrayList<Object>();
    							list.add(ProtocolMsg.MINESWEEPER_MASTER_EXIT);
    							mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list);
    							
    					}
    					
    					if(mineSweeperThreadList.get(hisRoomNum).size()==0) {
    						System.out.println("이제 아무도 없음");
    						gameAlreadyStartList.remove((Integer)hisRoomNum);  // 오류 날 수 있음.
    						System.out.println("그래서 얼레디리스트에서 없앰.");
    //						List<Object> list = new ArrayList<Object>();
    //						list.add(ProtocolMsg.MINESWEEPER_GAME_END);
    //						broadcasting(threadList, list);   // 이거 이제 필요없을 듯?
    						mineSweeperThreadList.remove((Integer)hisRoomNum);
    						idList.remove((Integer)hisRoomNum);
    						System.out.println("방 비어서  없애버림 ㄷㄷ");
    					}
    					
    					System.out.println("지뢰찾기 방 개수 : "+mineSweeperThreadList.size());
    				}else if(protocol.equals(ProtocolMsg.START_MINESWEEPER)) {
    					System.out.println("they want to start Minesweeper");
    					int hisRoomNum = (int)command.get(1);
    //					minesweeperAlreadyStart = true;
    					gameAlreadyStartList.add(hisRoomNum);
    					System.out.println("방번호 : " +  hisRoomNum);
    					
    					GameServer.minesweepOverInfoList.remove(hisRoomNum);  // 이걸로 다 지워질랑가 몰라
    					GameServer.minesweepOverInfoList.put(hisRoomNum, new ArrayList<ArrayList<Object> >());
    //					GameServer.minesweepOverInfoList.get(hisRoomNum).removeAll(c)
    					List<Object> list = new ArrayList<Object>();
    					char[][] mineMap = MakeMine.makeMine();
    					list.add(ProtocolMsg.MINESWEEPER_PRESTART);
    					list.add(mineMap);
    					Color[] colors = {Color.RED, Color.YELLOW, Color.BLUE, Color.GREEN};
    					int minesweeperNum = mineSweeperThreadList.get(hisRoomNum).size();
    					System.out.println(minesweeperNum +" " + hisRoomNum);
    					for(int i=0;i<minesweeperNum;i++) {
    						mineSweeperThreadList.get(hisRoomNum).get(i).iFailMinesweep=false;
    						
    						list.add(colors[i]);
    						
    						mineSweeperThreadList.get(hisRoomNum).get(i).sendMsg(list);
    						list.remove(2);
    					}
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {
    					System.out.println("he clicks safety zone");
    					int hisRoomNum = (int)command.get(5);
    //					int heClickRow = (int)command.get(1);
    //					int heClickCol = (int)command.get(2);
    //					Color hisColor = (Color)command.get(3);
    //					List<Object> list = new ArrayList<Object>();
    					broadcasting(mineSweeperThreadList.get(hisRoomNum), command);
    					
    					
    				}else if(protocol.equals(ProtocolMsg.USER_FINISH_MINESWEEPER)) {
    					System.out.println("user finish minesweeper");
    					
    					int hisRoomNum = (int)command.get(1);
    					
    					broadcasting(mineSweeperThreadList.get(hisRoomNum),command);
    					if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
    						List<Object> list3 = new ArrayList<>();
    						list3.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
    						list3.add(hisRoomNum);
    						mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list3);
    					}
    					
    					gameAlreadyStartList.remove((Integer)hisRoomNum);
    					
    				}else if(protocol.equals(ProtocolMsg.I_FAIL_MINESWEEP)) {
    					iFailMinesweep= true;
    					int hisRoomNum = (int)command.get(1);
    					boolean allFail = true;
    					
    					for(GameServerThread thread : mineSweeperThreadList.get(hisRoomNum)) {
    						if(!thread.iFailMinesweep) {
    							allFail=false;
    						}
    					}
    					if(allFail) {
    						List<Object> list = new ArrayList<Object>();
    						list.add(ProtocolMsg.MINESWEEP_ALL_FAIL);
    						
    						broadcasting(mineSweeperThreadList.get(hisRoomNum),list);
    						
    						if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
    							List<Object> list2 = new ArrayList<>();
    							list2.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
    							list2.add(hisRoomNum);
     							mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list2);
    						}
    						
    						gameAlreadyStartList.remove((Integer)hisRoomNum);
    					}
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEP_OVER_INFO)) {
    					int hisRoomNum = (int)command.get(5);
    					GameServer.minesweepOverInfoList.get(hisRoomNum).add(command);
    					
    					if(GameServer.minesweepOverInfoList.get(hisRoomNum).size()==mineSweeperThreadList.get(hisRoomNum).size()) {
    						Collections.sort(GameServer.minesweepOverInfoList.get(hisRoomNum), new Comparator<ArrayList<Object> >() {
    							@Override
    							public int compare(ArrayList<Object> o1, ArrayList<Object> o2) {
    								int cnt1 = (int)o1.get(4);
    								int cnt2 = (int)o2.get(4);
    								if(cnt1<cnt2) return 1;
    								else if(cnt1>cnt2) return -1;
    								else return 1;
    								
    								
    							}
    							
    						});
    						List<Object> infolistSpecial = new ArrayList<Object>();
    						infolistSpecial.add(ProtocolMsg.MINESWEEP_OVER_ALLINFO);
    //						infolist.add(GameServer.minesweepOverInfoList);
    						System.out.println(">>"+GameServer.minesweepOverInfoList.get(hisRoomNum));
    						List<Object> infoList = new ArrayList<Object>();
    						for(ArrayList<Object> tmp1 : GameServer.minesweepOverInfoList.get(hisRoomNum)) {
    							System.out.println((String)tmp1.get(1) + tmp1.get(3) + tmp1.get(4));
    							ArrayList<Object> tmp2 = new ArrayList<Object>();
    							tmp2.add(tmp1.get(1));
    							tmp2.add(tmp1.get(3));
    							tmp2.add(tmp1.get(4));
    							infoList.add(tmp2);
    						}
    						infolistSpecial.add(infoList);
    						
    						broadcasting(mineSweeperThreadList.get(hisRoomNum), infolistSpecial);
    						
    						ProtocolMsg.battleLogInsert(infolistSpecial, "MineSweeper");
    						
    					}
    					
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANT_RANKING)) {
    					List<Object> vNum = new ArrayList<Object>();
    					vNum.add(ProtocolMsg.VICTORY_NUM);
    					vNum.add(ProtocolMsg.getRank());
    					oos.writeObject(vNum);
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
    					List<Object> logList = new ArrayList<Object>();
    					logList.add(ProtocolMsg.I_WANT_BATTLELOG);
    					logList.add(ProtocolMsg.getBattleLog());
    					oos.writeObject(logList);
    					
    				}
    				
    				System.out.println(command.toString());
    			}
    			
    			
    			
    			
    			}catch(Exception e) {
    				System.out.println("연결 하나 끊김.");
    				e.printStackTrace();
    				threadList.remove(this);
    			}
    		
    	}
    ```
    
- **클라이언트 Thread에서 처리하는 명령들**
    
    ```java
    @Override
    	public void run() {
    		
    		makeLogin.setSize(500,430);
    		makeLogin.setVisible(true);
    		try {
    		
    		
    		while(true) {
    			try {
    				Object readObject= ois.readObject();
    				ArrayList<Object> command = (ArrayList<Object>)readObject; 
    				String protocol = (String)command.get(0);
    				System.out.println(protocol);
    				if(protocol.equals(ProtocolMsg.LOGIN_CHECK)) {
    					System.out.println("login check complete");
    					this.canLogin = (int)command.get(1);
    					
    					
    				}else if(protocol.equals(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER)) {
    					master= true;
    					
    					canMineSweeper=true;
    					myRoomNum = (int)command.get(1);
    					makeMineSweeper.roomNumLabel.setText(Integer.toString(myRoomNum+1)+"번 방");
    					makeMineSweeper.startBt.setEnabled(true);
    					
    				}else if(protocol.equals(ProtocolMsg.YOU_CAN_MINESWEEPER)) {
    					master=false;
    					
    					myRoomNum = (int)command.get(1);
    					makeMineSweeper.roomNumLabel.setText(Integer.toString(myRoomNum+1)+"번 방");
    					canMineSweeper =true;
    				}else if(protocol.equals(ProtocolMsg.YOU_CANNOT_MINESWEEPER)) {
    					System.out.println("시작못한다고 전령옴 ..");
    					master = false;
    					canMineSweeper=false;
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_MASTER_EXIT)) {
    					makeMineSweeper.startBt.setEnabled(true);
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_PRESTART)) {
    					System.out.println("시작 !! 지령받음.");
    					makeMineSweeper.cnt=0;
    					
    					makeMineSweeper.makeDialog();
    					makeMineSweeper.minesMap = (char[][])command.get(1);
    					for(int i=0;i<16;i++) {
    						for(int j=0;j<16;j++) {
    							if(makeMineSweeper.minesMap[i][j]=='X') {
    								makeMineSweeper.checkButtons[i][j]=1;
    							}
    						}
    					}
    					makeMineSweeper.myColor = (Color) command.get(2);
    				}else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {  // 여기서 0 처리 하면될듯
    					int heClickRow = (int)command.get(1);
    					int heClickCol = (int)command.get(2);
    					Color hisColor = (Color)command.get(3);
    					int heClickWhat = (char)command.get(4)-'0';
    //					makeMineSweeper.buttons[heClickRow][heClickCol].setEnabled(false);
    					makeMineSweeper.buttons[heClickRow][heClickCol].removeMouseListener(makeMineSweeper);
    					makeMineSweeper.buttons[heClickRow][heClickCol].removeActionListener(makeMineSweeper);
    					makeMineSweeper.buttons[heClickRow][heClickCol].setIcon(new ImageIcon(icons[heClickWhat]));
    					makeMineSweeper.buttons[heClickRow][heClickCol].setBorder(new LineBorder(hisColor,5));
    					makeMineSweeper.checkButtons[heClickRow][heClickCol]=1;
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_ALREADY_START)) {
    					master = false;
    					canMineSweeper=false;
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_GAME_END)) {
    					System.out.println("마인 종료 알림받음.");
    					canMineSweeper=true;
    					
    				}else if(protocol.equals(ProtocolMsg.USER_FINISH_MINESWEEPER)) {
    					System.out.println("게임 종료 알림 받음.");
    					
    					List<Object> list = new ArrayList<Object>();
    					list.add(ProtocolMsg.MINESWEEP_OVER_INFO);
    					list.add(makeLogin.myId);
    					list.add(makeLogin.myPw);
    					list.add(makeMineSweeper.myColor);
    					list.add(makeMineSweeper.cnt);
    					list.add(GameClientThread.myRoomNum);
    					makeLogin.oos.writeObject(list);   // curr
    					
    					
    					
    					
    					String[] str = {"확인"};
    					int confirm = JOptionPane.showOptionDialog(makeMineSweeper,"게임이 종료되었습니다. !!", "게임 오버", JOptionPane.OK_OPTION,
    							JOptionPane.INFORMATION_MESSAGE, null, str, str[0]);
    					if(confirm==JOptionPane.OK_OPTION) {
    						makeMineSweeper.buttonReset();
    						
    					}
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEP_ALL_FAIL)) {
    					System.out.println("minesweeper all fail");
    					
    					List<Object> list = new ArrayList<Object>();
    					list.add(ProtocolMsg.MINESWEEP_OVER_INFO);
    					list.add(makeLogin.myId);
    					list.add(makeLogin.myPw);
    					list.add(makeMineSweeper.myColor);
    					list.add(makeMineSweeper.cnt);
    					list.add(GameClientThread.myRoomNum);
    					makeLogin.oos.writeObject(list);   // curr
    					
    					
    					String[] str = {"확인"};
    					int confirm = JOptionPane.showOptionDialog(makeMineSweeper,"모두 실패하여 게임이 종료되었습니다.. 그것밖에 안됩니까들", "게임 오버", JOptionPane.OK_OPTION,
    							JOptionPane.INFORMATION_MESSAGE, null, str, str[0]);
    					if(confirm==JOptionPane.OK_OPTION) {
    						makeMineSweeper.buttonReset();
    					}
    					
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEP_OVER_ALLINFO)) {
    					System.out.println("game over all info get");
    					
    					for(int i=0;i<16;i++) {
    						for(int j=0;j<16;j++) {
    							makeMineSweeper.buttons[i][j].removeActionListener(makeMineSweeper);
    						}
    					}
    					
    					System.out.println(">>" + command.get(1));
    					makeMineSweeper.makeGameInfoDialog(command.get(1));
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.VICTORY_NUM)) {
    					System.out.println("get victory num");
    					System.out.println(command.get(1));
    					
    					makeMenu.makeRanking(command.get(1));
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
    					System.out.println("get battleLog");
    					
    					makeMenu.makeBattleLog(command.get(1));
    				}else if(protocol.equals(ProtocolMsg.SOMEONE_IN)) {
    					System.out.println("someone in");
    					
    					Vector<String> idlist2 = new Vector<>();
    //					System.out.println("commandget1's size : " + (ArrayList<String>)command.get(1));
    					for(int i=0;i<(int)command.get(1);i++) {
    						idlist2.add((String)command.get(i+2));
    					}
    					System.out.println(idlist2);
    					
    					makeMineSweeper.setRight(idlist2);
    				}else if(protocol.equals(ProtocolMsg.SOMEONE_EXIT)) {
    					System.out.println("someone exit");
    					
    					Vector<String> idlist2 = new Vector<>();
    //					System.out.println("commandget1's size : " + (ArrayList<String>)command.get(1));
    					for(int i=0;i<(int)command.get(1);i++) {
    						idlist2.add((String)command.get(i+2));
    					}
    					System.out.println(idlist2);
    					
    					makeMineSweeper.setRight(idlist2);
    					
    				}
    				
    				
    			} catch (ClassNotFoundException e) {
    				e.printStackTrace();
    			}
    		}
    		
    		}catch(IOException ioe) {
    			ioe.printStackTrace();
    		}
    		
    	
    	}
    ```
    

### 2**. DB 작업**

**2-1. 테이블**

- **ID를 키값으로 PW와 함께 login 테이블에 저장**
- **전적 테이블은 정수형 인덱스 btId를 키값으로 하고 시퀀스로 하나씩 증가하며 Insert**

```sql
create table login(
	id varchar(20) constraint login_pk primary key,
	pw varchar(20) constraint pw_nn not null
);

create table battleLog(
	btId number(7) CONSTRAINT battlelog_pk primary key,
	btGame varchar(20) constraint btgame_nn not null,
	btUserNum number(7) constraint btusernum_nn not null,
	btUserName1 varchar(20),
	btUserName2 varchar(20),
	btUserName3 varchar(20),
	btUserName4 varchar(20),
	btUserScore1 number(7),
	btUserScore2 number(7),
	btUserScore3 number(7),
	btUserScore4 number(7)
);

create sequence battlelog_seq;
```

2**-2. 로그인**

- **로그인 체크**
    
    ```java
    public static int canLogin(ArrayList<Object> command) {
    		
    		Connection conn = null;
    		Statement stmt = null;
    //		PreparedStatement pstmt = null;
    		ResultSet rs = null;
    		
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			String inputId = (String)command.get(1);
    			String inputPw = (String)command.get(2);
    			
    			String sql = "select * from login where id = '"+inputId+"'";
    			stmt= conn.createStatement();
    			rs=stmt.executeQuery(sql);
    			rs.next();
    			String getId = rs.getString("id");
    			if(getId == null) getId = "";
    			String getPw = rs.getString("pw");
    			if(getPw == null) getPw = "";
    //			rs.next();
    			if(inputId.equals(getId) && inputPw.equals(getPw)) {  
    				return 1;  // 로그인 성공
    			}else if(inputPw.equals("")) {
    				return 0;  // 로그인 정보 없음
    			}else if(!inputPw.equals(getPw)) {
    				return 2;  // 비밀번호 불일치
    			}
    			
    			
    			
    		} catch (ClassNotFoundException e) {
    			System.out.println("db 못찾겟는데요");
    		} catch(SQLException sqle) {
    			sqle.printStackTrace();
    		}finally{
    			try{if(rs!=null) rs.close();}catch(SQLException sqle) {}
    			try{if(stmt!=null) stmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    		
    		return 0;
    	}
    ```
    
- **회원가입 & 로그인**
    
    ```java
    public static void powerLogin(String id, String pw ) {
    		
    		Connection conn = null;
    		PreparedStatement pstmt = null;
    		
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			StringBuffer sb = new StringBuffer();
    			sb.append("insert into login values ( ? , ? )");
    			pstmt = conn.prepareStatement(sb.toString());
    			pstmt.setString(1, id);
    			pstmt.setString(2, pw);
    			
    			pstmt.executeUpdate();
    			System.out.println("유저 한명 추가요~");
    			
    		}catch(SQLException sqle) {
    			System.out.println("insert 안댐");
    		}catch(ClassNotFoundException cnfe) {
    			System.out.println("db 연결안댐");
    		}finally{
    			
    			try{if(pstmt!=null) pstmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    	}
    ```
    

**2-3. 전적관리**

- **게임 끝났을때 전적 Insert**
    
    ```java
    public static void battleLogInsert(List<Object> command1, String whatGame) {
    		Object command = command1.get(1);
    		List<ArrayList<Object>> infolist = (List<ArrayList<Object>>)command;
    		
    		Connection conn = null;
    		PreparedStatement pstmt = null;
    		
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			StringBuffer sb = new StringBuffer();
    			
    			sb.append("insert into battleLog values (battlelog_seq.nextVal ,?,?,?,?,?,?,?,?,?,? )");
    			pstmt = conn.prepareStatement(sb.toString());
    			pstmt.setString(1, whatGame);
    			pstmt.setInt(2, infolist.size());
    			pstmt.setString(3, null);
    			pstmt.setString(4, null);
    			pstmt.setString(5, null);
    			pstmt.setString(6, null);
    			pstmt.setInt(7, 0);
    			pstmt.setInt(8, 0);
    			pstmt.setInt(9, 0);
    			pstmt.setInt(10, 0);
    			if(infolist.size()>=1) {
    				pstmt.setString(3, (String)infolist.get(0).get(0));
    				pstmt.setInt(7, (int)infolist.get(0).get(2));
    			}
    			
    			if(infolist.size()>=2) {
    				pstmt.setString(4, (String)infolist.get(1).get(0));
    				pstmt.setInt(8, (int)infolist.get(1).get(2));
    			}
    			
    			if(infolist.size()>=3) {
    				pstmt.setString(5, (String)infolist.get(2).get(0));
    				pstmt.setInt(9, (int)infolist.get(2).get(2));
    			}
    			
    			if(infolist.size()>=4) {
    				pstmt.setString(6, (String)infolist.get(3).get(0));
    				pstmt.setInt(10, (int)infolist.get(3).get(2));
    			}
    			
    			
    			
    			pstmt.executeUpdate();
    			System.out.println("전적 하나 추가요 ~~ * ") ;
    			
    		}catch(SQLException sqle) {
    			System.out.println("insert 안댐");
    		}catch(ClassNotFoundException cnfe) {
    			System.out.println("db 연결안댐");
    		}finally{
    			
    			try{if(pstmt!=null) pstmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    		
    		
    	}
    ```
    
- **전적 보기**
    
    ```java
    public static List<List<Object>> getBattleLog() {
    		Connection conn = null;
    //		Statement stmt = null;
    		PreparedStatement pstmt = null;
    		ResultSet rs = null;
    		
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			
    			String sql = "select * from battleLog order by btid desc ";
    			pstmt= conn.prepareStatement(sql);
    			rs=pstmt.executeQuery();
    			
    			
    			rs = pstmt.executeQuery();
    			List<List<Object> > allLog = new ArrayList<List<Object>>();
    			while(rs.next()) {
    				List<Object> singleLog = new ArrayList<Object>();
    				int btid = rs.getInt(1);
    				System.out.println(btid);
    				String btGame = rs.getString(2);
    				int btUserNum = rs.getInt(3);
    				String btPersonId1 = rs.getString(4);
    				String btPersonId2 = rs.getString(5);
    				String btPersonId3 = rs.getString(6);
    				String btPersonId4 = rs.getString(7);
    				int btUserScore1 = rs.getInt(8);
    				int btUserScore2 = rs.getInt(9);
    				int btUserScore3 = rs.getInt(10);
    				int btUserScore4 = rs.getInt(11);
    				singleLog.add(btid);
    				singleLog.add(btGame);
    				singleLog.add(btUserNum);
    				singleLog.add(btPersonId1);
    				singleLog.add(btPersonId2);
    				singleLog.add(btPersonId3);
    				singleLog.add(btPersonId4);
    				singleLog.add(btUserScore1);
    				singleLog.add(btUserScore2);
    				singleLog.add(btUserScore3);
    				singleLog.add(btUserScore4);
    				
    				allLog.add(singleLog);
    				
    				
    			}
    			
    			return allLog;
    			
    			
    		} catch (ClassNotFoundException e) {
    			System.out.println("db 못찾겟는데요");
    		} catch(SQLException sqle) {
    			sqle.printStackTrace();
    		}finally{
    			try{if(rs!=null) rs.close();}catch(SQLException sqle) {}
    			try{if(pstmt!=null) pstmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    		
    		return null;
    	}
    ```
    
- **랭킹보기**
    
    ```java
    public static List<Object> getRank() {
    		Connection conn = null;
    		PreparedStatement pstmt = null;
    		ResultSet rs= null;
    		ArrayList<ArrayList<Object> > allRank = new ArrayList<>();
    		List<Object> list = new ArrayList<Object>();
    		
    		
    		String[] array = {"btusername1","btusername2","btusername3","btusername4"};
    		for(int i=0;i<4;i++) {
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			StringBuffer sb = new StringBuffer();
    			sb.append("select "+array[i]+" , count(*)  " + 
    					" from battlelog " + 
    					" where btgame='MineSweeper' and ? is not null  " + 
    					" group by  "+array[i] + 
    					" order by count(*) desc ");
    			pstmt = conn.prepareStatement(sb.toString());
    			pstmt.setString(1,array[i] );
    			
    		
    		
    			List<Object> rankrankList = new ArrayList<Object>();
    			
    			rs = pstmt.executeQuery();
    			while(rs.next()) {
    				String name = rs.getString(array[i]);
    				int cnt = rs.getInt("COUNT(*)");
    				int rank = i+1;
    				rankrankList.add(name);
    				rankrankList.add(rank);
    				rankrankList.add(cnt);
    			}
    			list.add(rankrankList);
    			
    			System.out.println(i+1+"등 랭킹 하나 불러옴 @@ ") ;
    			
    		}catch(SQLException sqle) {
    			sqle.printStackTrace();
    			System.out.println("rank get 안댐");
    		}catch(ClassNotFoundException cnfe) {
    			System.out.println("db 연결안댐");
    		}finally{
    			
    			try{if(pstmt!=null) pstmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    		}
    		
    		return list;
    		
    	}
    ```
    

### 3**. 클라이언트**

3**-1. 소켓 연결**

- 게임 클라이언트 실행시 열어뒀던 서버와 포트에 소켓 연결

```java
public GameClient() {
		
		try {
			socket = new Socket("localhost", 10303);
			GameClientThread thread = new GameClientThread(socket);
			thread.start();
		} catch (UnknownHostException e) {
			System.out.println("서버 없음");
			e.printStackTrace();
		} catch (IOException e) {
			System.out.println("연결 실패");
			e.printStackTrace();
		}
		
	}
	
	public static void main(String[] args) {
		new GameClient();
	}
```

**3-2. 클라이언트 Thread**

- **서버에 대한 응답 대기**
    
    ```java
    @Override
    	public void run() {
    		
    		makeLogin.setSize(500,430);
    		makeLogin.setVisible(true);
    		try {
    		
    		
    		while(true) {
    			try {
    				Object readObject= ois.readObject();
    				ArrayList<Object> command = (ArrayList<Object>)readObject; 
    				String protocol = (String)command.get(0);
    				System.out.println(protocol);
    				if(protocol.equals(ProtocolMsg.LOGIN_CHECK)) {
    					System.out.println("login check complete");
    					this.canLogin = (int)command.get(1);
    					
    					
    				}else if(protocol.equals(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER)) {
    					master= true;
    					
    					canMineSweeper=true;
    					myRoomNum = (int)command.get(1);
    					makeMineSweeper.roomNumLabel.setText(Integer.toString(myRoomNum+1)+"번 방");
    					makeMineSweeper.startBt.setEnabled(true);
    					
    				}else if(protocol.equals(ProtocolMsg.YOU_CAN_MINESWEEPER)) {
    					master=false;
    					
    					myRoomNum = (int)command.get(1);
    					makeMineSweeper.roomNumLabel.setText(Integer.toString(myRoomNum+1)+"번 방");
    					canMineSweeper =true;
    				}else if(protocol.equals(ProtocolMsg.YOU_CANNOT_MINESWEEPER)) {
    					System.out.println("시작못한다고 전령옴 ..");
    					master = false;
    					canMineSweeper=false;
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_MASTER_EXIT)) {
    					makeMineSweeper.startBt.setEnabled(true);
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_PRESTART)) {
    					System.out.println("시작 !! 지령받음.");
    					makeMineSweeper.cnt=0;
    					
    					makeMineSweeper.makeDialog();
    					makeMineSweeper.minesMap = (char[][])command.get(1);
    					for(int i=0;i<16;i++) {
    						for(int j=0;j<16;j++) {
    							if(makeMineSweeper.minesMap[i][j]=='X') {
    								makeMineSweeper.checkButtons[i][j]=1;
    							}
    						}
    					}
    					makeMineSweeper.myColor = (Color) command.get(2);
    				}else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {  // 여기서 0 처리 하면될듯
    					int heClickRow = (int)command.get(1);
    					int heClickCol = (int)command.get(2);
    					Color hisColor = (Color)command.get(3);
    					int heClickWhat = (char)command.get(4)-'0';
    //					makeMineSweeper.buttons[heClickRow][heClickCol].setEnabled(false);
    					makeMineSweeper.buttons[heClickRow][heClickCol].removeMouseListener(makeMineSweeper);
    					makeMineSweeper.buttons[heClickRow][heClickCol].removeActionListener(makeMineSweeper);
    					makeMineSweeper.buttons[heClickRow][heClickCol].setIcon(new ImageIcon(icons[heClickWhat]));
    					makeMineSweeper.buttons[heClickRow][heClickCol].setBorder(new LineBorder(hisColor,5));
    					makeMineSweeper.checkButtons[heClickRow][heClickCol]=1;
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_ALREADY_START)) {
    					master = false;
    					canMineSweeper=false;
    				}else if(protocol.equals(ProtocolMsg.MINESWEEPER_GAME_END)) {
    					System.out.println("마인 종료 알림받음.");
    					canMineSweeper=true;
    					
    				}else if(protocol.equals(ProtocolMsg.USER_FINISH_MINESWEEPER)) {
    					System.out.println("게임 종료 알림 받음.");
    					
    					List<Object> list = new ArrayList<Object>();
    					list.add(ProtocolMsg.MINESWEEP_OVER_INFO);
    					list.add(makeLogin.myId);
    					list.add(makeLogin.myPw);
    					list.add(makeMineSweeper.myColor);
    					list.add(makeMineSweeper.cnt);
    					list.add(GameClientThread.myRoomNum);
    					makeLogin.oos.writeObject(list);   // curr
    					
    					
    					
    					
    					String[] str = {"확인"};
    					int confirm = JOptionPane.showOptionDialog(makeMineSweeper,"게임이 종료되었습니다. !!", "게임 오버", JOptionPane.OK_OPTION,
    							JOptionPane.INFORMATION_MESSAGE, null, str, str[0]);
    					if(confirm==JOptionPane.OK_OPTION) {
    						makeMineSweeper.buttonReset();
    						
    					}
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEP_ALL_FAIL)) {
    					System.out.println("minesweeper all fail");
    					
    					List<Object> list = new ArrayList<Object>();
    					list.add(ProtocolMsg.MINESWEEP_OVER_INFO);
    					list.add(makeLogin.myId);
    					list.add(makeLogin.myPw);
    					list.add(makeMineSweeper.myColor);
    					list.add(makeMineSweeper.cnt);
    					list.add(GameClientThread.myRoomNum);
    					makeLogin.oos.writeObject(list);   // curr
    					
    					
    					String[] str = {"확인"};
    					int confirm = JOptionPane.showOptionDialog(makeMineSweeper,"모두 실패하여 게임이 종료되었습니다.. 그것밖에 안됩니까들", "게임 오버", JOptionPane.OK_OPTION,
    							JOptionPane.INFORMATION_MESSAGE, null, str, str[0]);
    					if(confirm==JOptionPane.OK_OPTION) {
    						makeMineSweeper.buttonReset();
    					}
    					
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.MINESWEEP_OVER_ALLINFO)) {
    					System.out.println("game over all info get");
    					
    					for(int i=0;i<16;i++) {
    						for(int j=0;j<16;j++) {
    							makeMineSweeper.buttons[i][j].removeActionListener(makeMineSweeper);
    						}
    					}
    					
    					System.out.println(">>" + command.get(1));
    					makeMineSweeper.makeGameInfoDialog(command.get(1));
    					
    					
    					
    				}else if(protocol.equals(ProtocolMsg.VICTORY_NUM)) {
    					System.out.println("get victory num");
    					System.out.println(command.get(1));
    					
    					makeMenu.makeRanking(command.get(1));
    					
    				}else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
    					System.out.println("get battleLog");
    					
    					makeMenu.makeBattleLog(command.get(1));
    				}else if(protocol.equals(ProtocolMsg.SOMEONE_IN)) {
    					System.out.println("someone in");
    					
    					Vector<String> idlist2 = new Vector<>();
    //					System.out.println("commandget1's size : " + (ArrayList<String>)command.get(1));
    					for(int i=0;i<(int)command.get(1);i++) {
    						idlist2.add((String)command.get(i+2));
    					}
    					System.out.println(idlist2);
    					
    					makeMineSweeper.setRight(idlist2);
    				}else if(protocol.equals(ProtocolMsg.SOMEONE_EXIT)) {
    					System.out.println("someone exit");
    					
    					Vector<String> idlist2 = new Vector<>();
    //					System.out.println("commandget1's size : " + (ArrayList<String>)command.get(1));
    					for(int i=0;i<(int)command.get(1);i++) {
    						idlist2.add((String)command.get(i+2));
    					}
    					System.out.println(idlist2);
    					
    					makeMineSweeper.setRight(idlist2);
    					
    				}
    				
    				
    			} catch (ClassNotFoundException e) {
    				e.printStackTrace();
    			}
    		}
    		
    		}catch(IOException ioe) {
    			ioe.printStackTrace();
    		}
    		
    	
    	}
    ```
    
- **전적 응답 받아 출력하는 예시**
    
    ```java
    //클라이언트에서 요청쪽 code
    }else if(selectedGame.equals("전적 보기")) {
    			List<Object> loglist = new ArrayList<Object>();
    			loglist.add(ProtocolMsg.I_WANT_BATTLELOG);
    			try {
    				MakeLogin.oos.writeObject(loglist);
    			} catch (IOException e1) {
    				e1.printStackTrace();
    			}
    }
    
    //서버 응답
    }else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
    		List<Object> logList = new ArrayList<Object>();
    		logList.add(ProtocolMsg.I_WANT_BATTLELOG);
    		logList.add(ProtocolMsg.getBattleLog());
    		oos.writeObject(logList);
    		
    	}
    
    //서버 응답2
    public static List<List<Object>> getBattleLog() {
    		Connection conn = null;
    //		Statement stmt = null;
    		PreparedStatement pstmt = null;
    		ResultSet rs = null;
    		
    		try {
    			Class.forName("oracle.jdbc.driver.OracleDriver");
    			System.out.println("db 연결 성공");
    			String url = "jdbc:oracle:thin:@localhost:1521/XEPDB1";
    			String dbId = "red";
    			String dbPw = "red";
    			conn = DriverManager.getConnection(url,dbId,dbPw);
    			
    			
    			String sql = "select * from battleLog order by btid desc ";
    			pstmt= conn.prepareStatement(sql);
    			rs=pstmt.executeQuery();
    			
    			
    			rs = pstmt.executeQuery();
    			List<List<Object> > allLog = new ArrayList<List<Object>>();
    			while(rs.next()) {
    				List<Object> singleLog = new ArrayList<Object>();
    				int btid = rs.getInt(1);
    				System.out.println(btid);
    				String btGame = rs.getString(2);
    				int btUserNum = rs.getInt(3);
    				String btPersonId1 = rs.getString(4);
    				String btPersonId2 = rs.getString(5);
    				String btPersonId3 = rs.getString(6);
    				String btPersonId4 = rs.getString(7);
    				int btUserScore1 = rs.getInt(8);
    				int btUserScore2 = rs.getInt(9);
    				int btUserScore3 = rs.getInt(10);
    				int btUserScore4 = rs.getInt(11);
    				singleLog.add(btid);
    				singleLog.add(btGame);
    				singleLog.add(btUserNum);
    				singleLog.add(btPersonId1);
    				singleLog.add(btPersonId2);
    				singleLog.add(btPersonId3);
    				singleLog.add(btPersonId4);
    				singleLog.add(btUserScore1);
    				singleLog.add(btUserScore2);
    				singleLog.add(btUserScore3);
    				singleLog.add(btUserScore4);
    				
    				allLog.add(singleLog);
    				
    				
    			}
    			
    			return allLog;
    			
    			
    		} catch (ClassNotFoundException e) {
    			System.out.println("db 못찾겟는데요");
    		} catch(SQLException sqle) {
    			sqle.printStackTrace();
    		}finally{
    			try{if(rs!=null) rs.close();}catch(SQLException sqle) {}
    			try{if(pstmt!=null) pstmt.close();}catch(SQLException sqle) {}
    			try{if(conn!=null) conn.close();}catch(SQLException sqle) {}
    			
    			
    		}
    		
    		return null;
    	}
    
    //클라이언트 프로토콜 명령 받음
    else if(protocol.equals(ProtocolMsg.I_WANT_BATTLELOG)) {
    		System.out.println("get battleLog");
    		
    		//데이터로 전적 UI구성
    		makeMenu.makeBattleLog(command.get(1));
    }
    
    ```
    

**3-3. 게임플레이**

- **지뢰찾기 0 터뜨리기**
    - **지뢰찾기를 진행하다 보면 한번에 여러개가 동시에 열리는 경우는 다음과 같다.**
    - **0을 눌렀을때 주변 8방향의 타일이 열고 그 타일이 다시 0이 있을 경우 같은 작업을 반복**
    
    ![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/650784f4-dab7-47dd-982e-36238b401cc5/d6190993-cefd-43ce-a65c-495db553ee36/Untitled.png)
    
    - **BFS로 그래프 탐색 후 클릭 처리**
    
    ```csharp
    //0 터뜨리기
    	if(minesMap[i][j]=='0') {
    		int[][] check = new int[16][16];
    		Queue<Pos> q = new LinkedList<Pos>();
    		q.add(new Pos(i,j));
    		check[i][j]=1;
    		while(!q.isEmpty()) {
    			int topRow = q.peek().row;
    			int topCol = q.peek().col;
    			int[] bp3 = {0,1,0,-1,1,1,-1,-1};
    			int[] bp4 = {1,0,-1,0,1,-1,-1,1};
    			for(int k=0;k<8;k++) {
    				int qwerRow = topRow+bp3[k];
    				int qwerCol = topCol+bp4[k];
    				if(qwerRow>=0 && qwerCol>=0 && qwerRow<16 && qwerCol<16) {
    					if(check[qwerRow][qwerCol]==0 && 
    							minesMap[qwerRow][qwerCol]!='X'
    							&&minesMap[qwerRow][qwerCol]!='0') {
    						
    						List<Object> listTo2 = new ArrayList<>();
    						listTo2.add(ProtocolMsg.CLICK_SAFEZONE);
    						listTo2.add(qwerRow);
    						listTo2.add(qwerCol);
    						listTo2.add(myColor);
    						listTo2.add(minesMap[qwerRow][qwerCol]);
    						listTo2.add(GameClientThread.myRoomNum);
    						check[qwerRow][qwerCol] = 1;
    						checkButtons[qwerRow][qwerCol]= 1;
    						cnt++;
    						try {
    							MakeLogin.oos.writeObject(listTo2);
    						} catch (IOException e1) {
    							e1.printStackTrace();
    						}
    						
    					}
    				}
    			}
    			
    			for(int p=0;p<8;p++) {
    				int nextRow = topRow + bp3[p];
    				int nextCol = topCol + bp4[p];
    				if(nextRow>=0 && nextCol >=0 && nextRow < 16 && nextCol<16) {
    					if(minesMap[nextRow][nextCol]=='0' && check[nextRow][nextCol]==0) {
    						q.add(new Pos(nextRow,nextCol));
    						
    						List<Object> listTo = new ArrayList<>();
    						listTo.add(ProtocolMsg.CLICK_SAFEZONE);
    						listTo.add(nextRow);
    						listTo.add(nextCol);
    						listTo.add(myColor);
    						listTo.add(minesMap[nextRow][nextCol]);
    						listTo.add(GameClientThread.myRoomNum);
    						check[nextRow][nextCol] = 1;
    						checkButtons[nextRow][nextCol]= 1;
    						cnt++;
    						try {
    							MakeLogin.oos.writeObject(listTo);
    						} catch (IOException e1) {
    							e1.printStackTrace();
    						}
    						
    						check[nextRow][nextCol]=1;
    					}
    				}
    			}
    			
    			q.poll();
    		}
    	}
    ```
    
- 방장이 시작하면 서버에 지뢰 맵 요청
- 지뢰맵 요청한 방에게 지뢰 맵 데이터 브로드캐스팅
- 지뢰가 아닌 부분을 클릭하면 다른 클라이언트에게 사실을 알리고 누를 수 없게 브로드캐스팅
    
    ```java
    //서버에서 세이프존을 클릭한 사실을 받고 그 방사람에게 브로드캐스팅
    else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {
    	System.out.println("he clicks safety zone");
    	int hisRoomNum = (int)command.get(5);
    	broadcasting(mineSweeperThreadList.get(hisRoomNum), command);
    }
    
    //클라이언트가 이 사실을 받고 그 버튼은 색을 입히고 더이상 누를 수 없게
    else if(protocol.equals(ProtocolMsg.CLICK_SAFEZONE)) {
    		int heClickRow = (int)command.get(1);
    		int heClickCol = (int)command.get(2);
    		Color hisColor = (Color)command.get(3);
    		int heClickWhat = (char)command.get(4)-'0';
    //					makeMineSweeper.buttons[heClickRow][heClickCol].setEnabled(false);
    		makeMineSweeper.buttons[heClickRow][heClickCol].removeMouseListener(makeMineSweeper);
    		makeMineSweeper.buttons[heClickRow][heClickCol].removeActionListener(makeMineSweeper);
    		makeMineSweeper.buttons[heClickRow][heClickCol].setIcon(new ImageIcon(icons[heClickWhat]));
    		makeMineSweeper.buttons[heClickRow][heClickCol].setBorder(new LineBorder(hisColor,5));
    		makeMineSweeper.checkButtons[heClickRow][heClickCol]=1;
    		
    	}
    ```
    
- 모든 플레이어가 실패하거나 모든 Safe Zone이 열렸을때 게임 종료
- 모든 플레이어에게 점수 브로드캐스팅하고 게임상태 종료 처리

```csharp
//서버에서 브로드캐스팅, 게임 종료 처리
else if(protocol.equals(ProtocolMsg.USER_FINISH_MINESWEEPER)) {
		System.out.println("user finish minesweeper");
		
		int hisRoomNum = (int)command.get(1);
		
		broadcasting(mineSweeperThreadList.get(hisRoomNum),command);
		if(!mineSweeperThreadList.get(hisRoomNum).isEmpty()) {
			List<Object> list3 = new ArrayList<>();
			list3.add(ProtocolMsg.YOU_ARE_MINESWEEPER_MASTER);
			list3.add(hisRoomNum);
			mineSweeperThreadList.get(hisRoomNum).get(0).sendMsg(list3);
		}
		
		gameAlreadyStartList.remove((Integer)hisRoomNum);
		
	}
```
