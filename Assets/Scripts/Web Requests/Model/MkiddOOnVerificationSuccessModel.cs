using System;

[Serializable]
public class MkiddOOnVerificationSuccessModel
{
    public bool success;
    public int status_code;
    public string refreshToken;
    public string accessToken;
    public int expiresIn;
    public int uid;
    public UserInfo user_info;
    public ChildInfo[] child_info;
}
[Serializable]
public class UserInfo
{
    public int id;
    public string msisdn;
    public string email;
    public string facebook_id;
    public string google_id;
    public string name;
    public string profile;
    public string reg_date;
    public int payment_status;
    public int last_paid_amount;
    public string last_payment_date;
    public string payment_valid_till;
    public string status;
    public int max_profile_limit;
    public string lastNotifyDate;
    public string agreementID;
    public string bkash_payment_number;
    public string remarks;
    public string updated_at;
    public string created_at;
}
[Serializable]
public class ChildInfo
{
    public int child_id;
    public string name;
    public string birth_date;
    public string profile_path;
    public AgeRange age_range;
    public int age_range_id;
}

[Serializable]
public class AgeRange
{
    public int key;
    public string value;
}