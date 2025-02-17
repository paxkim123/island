package project.db;

import java.util.ArrayList;
import java.util.Scanner;

public class DataBase {
	Scanner st = new Scanner(System.in);
	private static ArrayList<String[]> UserData = new ArrayList<>();
	public void set_USER(String Id, String Hasing_Password, String SALT) {
		String[] temp = {Id, Hasing_Password, SALT}; 
		UserData.add(temp);
	}
	
	public boolean check(String Id, String Hasing_Password) {
		for(int i = 0; i < UserData.size(); i++) {
			if(Id.equals(UserData.get(i)[0])) {
				if(Hasing_Password.equals(UserData.get(i)[1])) {
					return true;
				}
			}
		}
		return false;
	}
	public boolean check_Id(String id) { //아이디 중복체크
		for(int i = 0; i < UserData.size(); i++) {
			if(id.equals(UserData.get(i)[0])) {
				return true;
			}
		}
		return false;
	}
	public String get_SALT(String Id) {
		String err = null;
		for(int i = 0; i < UserData.size();i++) {
			if(Id.equals(UserData.get(i)[0])) {
				return UserData.get(i)[2];
			}
		}
		return err;
	}
	public String toString() {
		StringBuilder sb = new StringBuilder();
		for(String[] temp: UserData) {
			sb.append("Id: "+temp[0]+"\tPassword: "+temp[1]+"\tSalt: "+temp[2]).append("\n");
		}
		return sb.toString();
	}
	
	public boolean login_admin() {
		System.out.print("아이디 입력: ");
		String id = st.nextLine();
		System.out.print("비밀번호 입력: ");
		int password = st.nextInt();
		if(!id.equals("admin")) {
			System.out.println("아이디가 틀렸습니다. 다시 입력해주세요");
			return false;
		}
		else if(password != 1234) {
			System.out.println("비밀번호가 틀렸습니다. 다시 입력해주세요");
			return false;
		}
		else {
			System.out.println("관리자 계정 로그인에 성공했습니다.");
			return true;
		}
	}
}