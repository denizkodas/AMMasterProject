using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AMMasterProject.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;

namespace AMMasterProject.Helpers
{
    public class WebsettingHelper
    {

        #region Model
        private readonly MyDbContext _dbContext;

        #endregion


        #region DI
        public WebsettingHelper(MyDbContext context)
        {
            _dbContext = context;

        }
        #endregion


        #region WebsetttingsJsonCreatorandUpdate

      
        public string GetWebsettingJson(string key)
        {
            Websetting websetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == key);

            if (websetting != null && !string.IsNullOrEmpty(websetting.WebsettingValue))
            {
                return websetting.WebsettingValue;
            }

            return null; // Return null if the key or JSON value is not found
        }


        public string UpdateWebsettingJson(string key, string jsonData)
        {
            try
            {
                // Deserialize the JSON data into a dictionary
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

                if (data != null && data.Count > 0)
                {
                    // Create a new JObject and populate it with the submitted values
                    var submittedObject = new JObject();

                    foreach (var entry in data)
                    {
                        submittedObject.Add(new JProperty(entry.Key, entry.Value));
                    }

                    // Convert the submittedObject to JSON
                    var json = submittedObject.ToString();

                    Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == key);

                    if (existingSetting != null)
                    {
                        // Update the existing record with the new JSON value
                        existingSetting.WebsettingValue = json;

                        _dbContext.Websettings.Update(existingSetting);
                        _dbContext.SaveChanges();

                        return "update";
                    }
                    else
                    {
                        // Create a new record with the JSON value
                        var newSetting = new Websetting
                        {
                            WebsettingKey = key,
                            WebsettingValue = json
                        };

                        _dbContext.Websettings.Add(newSetting);
                        _dbContext.SaveChanges();

                        return "insert";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the process
                return "Error: " + ex.Message;
            }

            return "Invalid JSON data" ;
        }
        public string UpdateWebsettingJsonList(string key, string jsonData)
        {
            try
            {
               

                    Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == key);

                    if (existingSetting != null)
                    {
                        // Update the existing record with the new JSON value
                        existingSetting.WebsettingValue = jsonData;

                        _dbContext.Websettings.Update(existingSetting);
                        _dbContext.SaveChanges();

                        return "update";
                    }
                    else
                    {
                        // Create a new record with the JSON value
                        var newSetting = new Websetting
                        {
                            WebsettingKey = key,
                            WebsettingValue = jsonData
                        };

                        _dbContext.Websettings.Add(newSetting);
                        _dbContext.SaveChanges();

                        return "insert";
                    }
                
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the process
                return "Error: " + ex.Message;
            }

            return "Invalid JSON data";
        }

        public string DeletedJson(string key, string jsonData)
        {
            try
            {
                Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == key);

                if (existingSetting != null)
                {

                    existingSetting.WebsettingValue = jsonData;

                    _dbContext.Websettings.Update(existingSetting);
                    _dbContext.SaveChanges();



                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the process
                return "Error: " + ex.Message;
            }

            return "Invalid JSON data";
        }

        #endregion

       

        #region AdminAddressSecondary-JsonCreator
        public string addressmetadata(string addressguid, string addressid, string address, string type, string storename, string contact, string email, string latitude, string longitude, string country, string state, string city, string zipcode, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<AdminAddressMetaData> existingMetadata = JsonConvert.DeserializeObject<List<AdminAddressMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(addressguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                addressguid = Guid.NewGuid().ToString();
                addressid = nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new AdminAddressMetaData
                {
                    AddressGUID = Guid.Parse(addressguid),
                    AddressID = int.Parse(addressid),
                    Address = address,
                    Type = type,
                    StoreName =storename,
                    ContactNumber = contact,
                    Email = email,
                    Latitude = latitude,
                    Longitude = longitude,
                    Country = country,
                    State = state,
                    City = city,
                    ZipCode = zipcode,

                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                AdminAddressMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.AddressGUID.ToString() == addressguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record

                    existingRecord.Address = address;
                    existingRecord.Type = type;
                    existingRecord.StoreName = storename;
                    existingRecord.ContactNumber = contact;
                    existingRecord.Email = email;
                    existingRecord.Latitude = latitude;
                    existingRecord.Longitude = longitude;
                    existingRecord.Country = country;
                    existingRecord.State = state;
                    existingRecord.City = city;
                    existingRecord.ZipCode = zipcode;


                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }

        #endregion

        #region AdminAddressSecondary-Deserialized
        public List<AdminAddressMetaData> ParseMetaDataAddressList(string json)
        {
            if (json == null)
            {
                return new List<AdminAddressMetaData>(); // Return an empty list
            }

            List<AdminAddressMetaData> parsedData = JsonConvert.DeserializeObject<List<AdminAddressMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion



        #region HTMLSave
        public string updatehtmlcontent(string merge, string html, string css, string json, int pagenameid, int loginid)
        {
            PageName update = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);
            if (update != null)
            {
                update.PageDescription = merge;
                update.PageHTML = html;
                update.PageCSS = css;
                update.PageJson = json;
                update.InsertDate = DateTime.Now;
                update.ProfileId = loginid;

                _dbContext.PageNames.Update(update);

                _dbContext.SaveChanges();




               

            }
            return "success";
            
            //else
            //{
            //    PageDetail insert = new PageDetail();

            //    insert.PageNameId = pagenameid;
            //    insert.PageDescription = merge;
            //    insert.PageHTML = html;
            //    insert.PageCSS = css;
            //    insert.PageJson = json;
            //    insert.IsPublish = true;
            //    insert.ProfileId = loginid;
            //    _dbContext.PageDetails.Add(insert);
            //    _dbContext.SaveChanges();






            //    return "success";

            //}
        }
        #endregion

      

    }
}
