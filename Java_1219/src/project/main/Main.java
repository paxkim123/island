package project.main;

import java.util.Scanner;

import project.user.User;

public class Main {
	static Scanner st = new Scanner(System.in);
	static User user = new User();
	public static void main(String[] args) throws Exception{
		
		while (true) {  
		    System.out.println("메뉴 선택: 1. 회원가입 2. 로그인 3. 종료 4. 데이터베이스 조회");
		    String number = st.nextLine();  
		    switch (number) {
		        case "1":
		            System.out.print("아이디 입력: ");
		            String id = st.nextLine();
		            while(true) {
		            	if(user.check_UserId(id)==0) {
		            		System.out.println("");
		            		System.out.print("아이디 입력: ");
		                    id = st.nextLine();
		                  
		            		continue;
		            	}
		            	else
		            		break;
		            }
		            System.out.print("비밀번호 입력: ");
		            String password = st.nextLine();
		            user.set_User(id, password.getBytes());
		            System.out.println("회원가입이 완료되었습니다. 메인 화면으로 돌아갑니다.");
		            break;  
		            
		        case "2":
		            log_in();
		            break;   

		        case "3":
		            System.out.println("프로그램을 종료합니다.");
		            return;  

		        case "4":
		            user.get_DataBase();
		            break;  

		        case "st_tab":
		        
		      
		            break;
		            
		        default:
		            System.out.println("잘못 입력하였습니다.");
		            break;  
		    }
		}
	}
	
	static void make_user() throws Exception{
		System.out.print("아이디 입력: ");
		String id = st.nextLine();
		System.out.print("비밀번호 입력: ");
		String password = st.nextLine();
		user.set_User(id, password.getBytes());
	}
	
	static void log_in() throws Exception{
		System.out.print("아이디 입력");
		String id = st.nextLine();
		System.out.print("비밀번호 입력");
		String password = st.nextLine();
		user.get_User(id, password.getBytes());
	}
}
	

