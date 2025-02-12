package project.db;

import java.util.ArrayList;

public class DataBase {
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
}
