package project.main;

import java.util.Scanner;

import project.db.DataBase;
import project.user.User;



public class Main {
		static Scanner st = new Scanner(System.in);
		static User user = new User();
		static DataBase db = new DataBase();
	public static void main(String[] args) throws Exception{
		
		while (true) {  
		    System.out.println("메뉴 선택: 1. 회원가입 2. 로그인 3. 종료 4. 데이터베이스 조회");
		    String number = st.nextLine();  
		    switch (number) {
		        case "1":
		        	
		            make_User();
		            break;  
		            
		        case "2":
		        	
		            log_in();
		            break;   

		        case "3":
		        	
		            System.out.println("프로그램을 종료합니다.");
		            return;  

		        case "4":
		        	
		        	if(db.login_admin()){
		        		user.get_DataBase();
		        	}
		        	
		            break;  

		        case "st_tab":
		        	
		            break;
		            
		        default:
		            System.out.println("잘못 입력하였습니다.");
		            break;  
		    }
		}
	}
	
	public static boolean check_UserPw(String pw) {
	    int length = pw.length();
	    int count_SpecialCharacter = 0;
	    int count_UpperCharacter = 0;
	    int count_LowerCharacter = 0;
	    String specialCharacters = "!@#$%^&*()-+=?/<>~{}[]";
	    
	    // 오류 메시지를 누적하여 한 번에 출력
	    StringBuilder errorMessage = new StringBuilder();
	    
	    for (char c : pw.toCharArray()) {
	        if (Character.isUpperCase(c)) 
	        	count_UpperCharacter++;
	        if (Character.isLowerCase(c)) 
	        	count_LowerCharacter++;
	        if (specialCharacters.indexOf(c) >= 0) 
	        	count_SpecialCharacter++;
	    }//주어진 문자열을 toCharArray() 메서드로 캐릭터 배열을 만든 다음 for each 구문을 사용하였다.

	    if (length < 8 || length >= 20) {
	        System.out.println("패스워드는 8자리 이상 20자리 미만이어야 합니다.");
	        return false;
	    }
	    
	    if (count_SpecialCharacter < 2) {
	        System.out.println("패스워드에는 특수문자가 2개 이상 포함되어야 합니다.");
	        return false;
	    }
	    
	    if (count_UpperCharacter < 1) {
	        System.out.println("패스워드에는 영어 대문자가 1개 이상 포함되어야 합니다.");
	        return false;
	    }
	    if (count_LowerCharacter < 1) {
	        System.out.println("패스워드에는 영어 소문자가 1개 이상 포함되어야 합니다.");
	        return false;
	    }
	    
	    // 오류가 있으면 메시지를 출력하고 false 반환
	    if (errorMessage.length() > 0) {
	        System.out.print(errorMessage.toString());
	        return false;
	    }

	    System.out.println("패스워드 조건에 부합합니다. 회원가입을 계속 진행합니다.");
	    return true;
	}
	
	static void make_User() throws Exception{
		System.out.print("아이디 입력: ");
	    String id = st.nextLine();
	    
	    while (!user.check_UserId(id)) {  
	        System.out.print("아이디 입력: ");
	        id = st.nextLine();
	    }//false 값일 경우 계속 반복한다. 즉, true가 되면 while을 빠져나오게 되므로

	    String password;
	    do {
	        System.out.print("비밀번호 입력: ");
	        password = st.nextLine();
	    } while (!check_UserPw(password));

	    user.set_User(id, password.getBytes());  // 해싱을 사용하는 것이 더 좋음
	    System.out.println("회원가입이 완료되었습니다. 메인 화면으로 돌아갑니다.");
	}
	
	
	static void log_in() throws Exception{
		System.out.print("아이디 입력: ");
		String id = st.nextLine();
		System.out.print("비밀번호 입력: ");
		String password = st.nextLine();
		user.get_User(id, password.getBytes());
	}
}
