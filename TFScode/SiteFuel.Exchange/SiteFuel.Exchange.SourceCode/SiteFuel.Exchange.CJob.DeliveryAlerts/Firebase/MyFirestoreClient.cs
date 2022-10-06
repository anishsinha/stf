using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Grpc.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    public class MyFirestoreClient
    {
        private FirestoreDb _fireStoreDb = null;
        private FirebaseConfiguration _firebaseConfiguration;

        public MyFirestoreClient()
        {
            var firebaseDomain = new FirebaseDomain();
            _firebaseConfiguration = Task.Run(() => firebaseDomain.GetFirebaseConfigurationAsync()).Result;
        }

        public MyFirestoreClient(FirebaseConfiguration firebaseConfiguration)
        {
            _firebaseConfiguration = firebaseConfiguration;
        }

        public void ListenForInvoiceDropChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.CollectionName);//.WhereGreaterThan("UpdateTime", _lastUpdateTime);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.LastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");

                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);
                var newDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (newDocuments.Any())
                {
                    _lastUpdateTime = newDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }
                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {newDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in newDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForChanges", $"New document received: {change.Document.Id}");

                            ProcessInvoiceDropDocument(firebaseDomain, change);
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Modified)
                        {
                            Console.WriteLine($"Document modified: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForChanges", $"Document modified: {change.Document.Id}");
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Removed)
                        {
                            Console.WriteLine($"Document removed: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForChanges", $"Document removed: {change.Document.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenForChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }

        private void ProcessInvoiceDropDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessInvoiceDropDocument", $"Processing new document received: {change.Document.Id}");

            var dropObject = change.Document.ConvertTo<MobileInvoiceViewModel>();
            var invoiceViewModel = dropObject.ToInvoiceViewModelNew();
            InvoiceDomain invoiceDomain = new InvoiceDomain();
            if (invoiceViewModel.Drops.SelectMany(t => t.Assets).Any(t1 => t1.IsOfflineMode && t1.PreDip > 0 && t1.PostDip > 0 && t1.TankScaleMeasurement > (int)Utilities.TankScaleMeasurement.None))
            {
                invoiceDomain.UpdateDropQuantityByPrePostDip(invoiceViewModel);
            }
            Console.WriteLine($"Mapped new document to invoiceViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessInvoiceDropDocument", $"Mapped new document to invoiceViewModel: {change.Document.Id}");

            if (invoiceViewModel.DivertedDrops.Any())
            {
                Task.Run(() => invoiceDomain.UpdateDiversionDropDelieveryRequests(invoiceViewModel)).Wait();
            }
            var response = Task.Run(() => firebaseDomain.AddDocumentToQueueMessage(invoiceViewModel, dropObject.InvoiceStatusId)).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved invoiceViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessInvoiceDropDocument", $"Saved invoiceViewModel to azure storage: {change.Document.Id}");

                DeleteFirestoreInvoiceDropDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                firebaseDomain.SetInvoiceDropLastUpdatedDateTimeAsync(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to saved invoiceViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessInvoiceDropDocument", $"Failed to saved invoiceViewModel to azure storage: {change.Document.Id}");
            }
        }

        public void ListenPreLoadBolForChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.PreLoadBolCollectionName);//.WhereGreaterThan("UpdateTime", _lastUpdateTime);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.PreLoadBolLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenPreLoadBolForChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenPreLoadBolForChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");

                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);
                var newDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (newDocuments.Any())
                {
                    _lastUpdateTime = newDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }
                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {newDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in newDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenPreLoadBolForChanges", $"New document received: {change.Document.Id}");

                            ProcessPreLoadBolDocument(firebaseDomain, change);
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Modified)
                        {
                            Console.WriteLine($"Document modified: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenPreLoadBolForChanges", $"Document modified: {change.Document.Id}");
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Removed)
                        {
                            Console.WriteLine($"Document removed: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenPreLoadBolForChanges", $"Document removed: {change.Document.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenPreLoadBolForChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }

        private void ProcessPreLoadBolDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPreLoadBolDocument", $"Processing new document received: {change.Document.Id}");

            var preLoadBolObject = change.Document.ConvertTo<MobilePreLoadBolViewModel>();
            var preLoadBolViewModel = preLoadBolObject.ToPreLoadBolViewModel();
            Console.WriteLine($"Mapped new document to preLoadBolViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPreLoadBolDocument", $"Mapped new document to preLoadBolViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.SavePreLoadBolDetails(new List<PreLoadBolViewModel> { preLoadBolViewModel })).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved preLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPreLoadBolDocument", $"Saved preLoadBolViewModel to azure storage: {change.Document.Id}");

                DeleteFirestorePreLoadBolDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                firebaseDomain.SetPreLoadBolLastUpdatedDateTimeAsync(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to saved preLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPreLoadBolDocument", $"Failed to saved preLoadBolViewModel to azure storage: {change.Document.Id}");
            }
        }

        private FirestoreDb GetFirestoreDb()
        {
            if (_fireStoreDb == null)
            {
                var serviceAcct = GoogleCredential.FromJson(_firebaseConfiguration.ServiceAccountJson);
                Channel channel = new Channel(FirestoreClient.DefaultEndpoint.Host, FirestoreClient.DefaultEndpoint.Port, serviceAcct.ToChannelCredentials());
                FirestoreClient client = FirestoreClient.Create(channel);

                _fireStoreDb = FirestoreDb.Create(_firebaseConfiguration.ProjectId, client);
            }
            return _fireStoreDb;
        }

        private async Task DeleteFirestoreInvoiceDropDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.CollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestoreInvoiceDropDocumentAsync", ex.Message, ex);
            }
        }

        private async Task DeleteFirestorePreLoadBolDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.PreLoadBolCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestorePreLoadBolDocumentAsync", ex.Message, ex);
            }
        }
        private async Task DeleteFirestorePickupBOLRetainDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.PickupBOLRetainCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestorePickupBOLRetainDocumentAsync", ex.Message, ex);
            }
        }

        private async Task DeleteFirestoreCancelScheduleDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.CancelledScheduleCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestoreCanceledScheduleDocumentAsync", ex.Message, ex);
            }
        }


        #region Edit/delete pre load bol details

        public void ListenForEditedPreLoadBolChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.EditedPreLoadBolCollectionName);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.EditedPreLoadBolLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForEditedPreLoadBolChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForEditedPreLoadBolChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");
                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);

                var bolDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (bolDocuments.Any())
                {
                    _lastUpdateTime = bolDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }

                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {bolDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in bolDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForEditedPreLoadBolChanges", $"New document received: {change.Document.Id}");
                            ProcessEditedPreLoadBolDocument(firebaseDomain, change);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenForEditedPreLoadBolChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }

        private void ProcessEditedPreLoadBolDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessEditedPreLoadBolDocument", $"Processing new document received: {change.Document.Id}");

            var editedPreLoadBolObject = change.Document.ConvertTo<EditPreLoadBolFireBaseViewModel>();
            var editedPreLoadBolViewModel = editedPreLoadBolObject.ToEditPreLoadBolViewModel();

            Console.WriteLine($"Mapped new document to edit editedPreLoadBolViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessEditedPreLoadBolDocument", $"Mapped new document to editedPreLoadBolViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.SaveEditedPreLoadBolDetails(editedPreLoadBolViewModel)).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved editedPreLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessEditedPreLoadBolDocument", $"Saved editedPreLoadBolViewModel to azure storage: {change.Document.Id}");

                DeleteFirestoreEditedPreLoadBolDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                var helperDomain = new HelperDomain();
                helperDomain.SetEditedPreLoadBolLastSyncDateTime(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to save editedPreLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessEditedPreLoadBolDocument", $"Failed to save editedPreLoadBolViewModel to azure storage: {change.Document.Id}");
            }
        }

        private async Task DeleteFirestoreEditedPreLoadBolDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.EditedPreLoadBolCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestoreEditedPreLoadBolDocumentAsync", ex.Message, ex);
            }
        }

        public void ListenForDeletedPreLoadBolChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.DeletedPreLoadBolCollectionName);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.DeletedPreLoadBolLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForDeletedPreLoadBolChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForDeletedPreLoadBolChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");
                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);

                var bolDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (bolDocuments.Any())
                {
                    _lastUpdateTime = bolDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }

                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {bolDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in bolDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForDeletedPreLoadBolChanges", $"New document received: {change.Document.Id}");
                            ProcessDeletedPreLoadBolDocument(firebaseDomain, change);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenForDeletedPreLoadBolChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }

        private void ProcessDeletedPreLoadBolDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessDeletedPreLoadBolDocument", $"Processing new document received: {change.Document.Id}");

            var deletedPreLoadBolObject = change.Document.ConvertTo<EditPreLoadBolFireBaseViewModel>();
            var deletedPreLoadBolViewModel = deletedPreLoadBolObject.ToEditPreLoadBolViewModel();

            Console.WriteLine($"Mapped new document to edit deletedPreLoadBolViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessDeletedPreLoadBolDocument", $"Mapped new document to deletedPreLoadBolViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.UpdateDeletedPreLoadBolDetails(deletedPreLoadBolViewModel)).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved deletedPreLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessDeletedPreLoadBolDocument", $"Saved deletedPreLoadBolViewModel to azure storage: {change.Document.Id}");

                DeleteFirestoreDeletedPreLoadBolDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                var helperDomain = new HelperDomain();
                helperDomain.SetDeletedPreLoadBolLastSyncDateTime(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to save deletedPreLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessDeletedPreLoadBolDocument", $"Failed to save deletedPreLoadBolViewModel to azure storage: {change.Document.Id}");
            }
        }

        private async Task DeleteFirestoreDeletedPreLoadBolDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.DeletedPreLoadBolCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestoreDeletedPreLoadBolDocumentAsync", ex.Message, ex);
            }
        }

        #endregion Edit/delete pre load bol details


        #region fuel retain details
        public void ListenForFuelRetainChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.FuelRetainCollectionName);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.FuelRetainLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForFuelRetainChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForFuelRetainChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");
                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);

                var fuelRetainDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (fuelRetainDocuments.Any())
                {
                    _lastUpdateTime = fuelRetainDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }

                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {fuelRetainDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in fuelRetainDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForFuelRetainChanges", $"New document received: {change.Document.Id}");
                            ProcessFuelRetainDocument(firebaseDomain, change);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenForFuelRetainChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }

        private void ProcessFuelRetainDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessFuelRetainDocument", $"Processing new document received: {change.Document.Id}");

            var fuelRetainObject = change.Document.ConvertTo<MobileFuelRetainViewModel>();
            var fuelRetainViewModel = fuelRetainObject.ToFuelRetainViewModel();

            Console.WriteLine($"Mapped new document to edit fuelRetainViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessFuelRetainDocument", $"Mapped new document to fuelRetainViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.SaveFuelRetainDetails(fuelRetainViewModel)).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved fuelRetainViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessFuelRetainDocument", $"Saved fuelRetainViewModel to azure storage: {change.Document.Id}");

                DeleteFirestoreFuelRetainDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                var helperDomain = new HelperDomain();
                helperDomain.SetFuelRetainLastSyncDateTime(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to save fuelRetainViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessFuelRetainDocument", $"Failed to save fuelRetainViewModel to azure storage: {change.Document.Id}");
            }
        }

        private void ProcessCancelledScheduleDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessCancelledScheduleDocument", $"Processing new document received: {change.Document.Id}");

            var cancelledScheduleObject = change.Document.ConvertTo<MobileCanceledScheduleModel>();
            var cancelledScheduleViewModel = cancelledScheduleObject.ToCancelScheduleViewModel();
            Console.WriteLine($"Mapped new document to ProcessCancelledScheduleDocumentViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessCancelledScheduleDocument", $"Mapped new document to ProcessPickupRetainDocumentViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.SaveCancelledScheduleDetails(cancelledScheduleViewModel)).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessCancelledScheduleDocument", $"Processed CanceledScheduleDocument: {change.Document.Id}");

                DeleteFirestoreCancelScheduleDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                firebaseDomain.SetPickupRetainLastUpdatedDateTimeAsync(updatedDateTime).Wait();
            }
            else
            {
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessCancelledScheduleDocument", $"Failed to process cancelledscheduledocument: {change.Document.Id}");
            }
        }

        private void ProcessPickupRetainDocument(FirebaseDomain firebaseDomain, Google.Cloud.Firestore.DocumentChange change)
        {
            Console.WriteLine($"Processing new document received: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPickupRetainDocument", $"Processing new document received: {change.Document.Id}");

            var preLoadBolObject = change.Document.ConvertTo<MobilePreLoadBolViewModel>();
            var preLoadBolViewModel = preLoadBolObject.ToPickupBolRetainViewModel();
            Console.WriteLine($"Mapped new document to ProcessPickupRetainDocumentViewModel: {change.Document.Id}");
            LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPickupRetainDocument", $"Mapped new document to ProcessPickupRetainDocumentViewModel: {change.Document.Id}");

            var response = Task.Run(() => firebaseDomain.SavePickupBolRetainDetails(new List<PreLoadBolViewModel> { preLoadBolViewModel })).Result;

            if (response.StatusCode == Utilities.Status.Success)
            {
                Console.WriteLine($"Saved ProcessPickupRetainDocumentViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPickupRetainDocument", $"Saved ProcessPickupRetainDocumentViewModel to azure storage: {change.Document.Id}");

                DeleteFirestorePickupBOLRetainDocumentAsync(change.Document.Id).Wait();
                var updatedDateTime = change.Document.UpdateTime.Value.ToDateTimeOffset().ToString();
                firebaseDomain.SetPickupRetainLastUpdatedDateTimeAsync(updatedDateTime).Wait();
            }
            else
            {
                Console.WriteLine($"Failed to saved preLoadBolViewModel to azure storage: {change.Document.Id}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ProcessPickupRetainDocument", $"Failed to saved ProcessPickupRetainDocumentViewModel to azure storage: {change.Document.Id}");
            }
        }

        private async Task DeleteFirestoreFuelRetainDocumentAsync(string documentId)
        {
            try
            {
                var fdb = GetFirestoreDb();
                var docRef = fdb.Collection(_firebaseConfiguration.FuelRetainCollectionName).Document(documentId);
                await docRef.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MyFirestoreClient", "DeleteFirestoreFuelRetainDocumentAsync", ex.Message, ex);
            }
        }

        #endregion fuel retain details

        #region SavePickupBOLRetainDetails
        public void ListenPickupBolRetainForChanges()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.PickupBOLRetainCollectionName);//.WhereGreaterThan("UpdateTime", _lastUpdateTime);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.PickupBOLRetainLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", $"Snapshot Received. Changes Count: {snapshot.Count}");

                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);
                var newDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (newDocuments.Any())
                {
                    _lastUpdateTime = newDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }
                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {newDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in newDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", $"New document received: {change.Document.Id}");

                            ProcessPickupRetainDocument(firebaseDomain, change);
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Modified)
                        {
                            Console.WriteLine($"Document modified: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", $"Document modified: {change.Document.Id}");
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Removed)
                        {
                            Console.WriteLine($"Document removed: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", $"Document removed: {change.Document.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListePickupBolRetainForChanges: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }
        #endregion

        #region Schedule Cancel
        public void ListenForCancelledSchedule()
        {
            var _db = GetFirestoreDb();
            Query query = _db.Collection(_firebaseConfiguration.CancelledScheduleCollectionName);//.WhereGreaterThan("UpdateTime", _lastUpdateTime);

            var _lastUpdateTime = Timestamp.FromDateTimeOffset(_firebaseConfiguration.CancelledScheduleLastUpdatedDateTime);
            FirestoreChangeListener listener = query.Listen(snapshot =>
            {
                Console.WriteLine();
                if (snapshot == null)
                {
                    Console.WriteLine("Null Snapshot Received. No need to process.");
                    LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForCancelledSchedule", "Null Snapshot Received. No need to process.");
                    return;
                }

                Console.WriteLine($"Snapshot Received. Changes Count: {snapshot.Count}");
                LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForCancelledSchedule", $"Snapshot Received. Changes Count: {snapshot.Count}");

                Console.WriteLine("Previous UpdateTime: {0}", _lastUpdateTime);
                var newDocuments = snapshot.Changes.Where(t => t.Document.UpdateTime > _lastUpdateTime).ToList();
                if (newDocuments.Any())
                {
                    _lastUpdateTime = newDocuments.Max(t => t.Document.UpdateTime ?? _lastUpdateTime);
                }
                Console.WriteLine("Current UpdateTime: {0}", _lastUpdateTime);
                Console.WriteLine($"Documents received count: {newDocuments.Count}");

                var firebaseDomain = new FirebaseDomain();
                foreach (Google.Cloud.Firestore.DocumentChange change in newDocuments)
                {
                    try
                    {
                        if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Added)
                        {
                            Console.WriteLine($"New document received: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForCancelledSchedule", $"New document received: {change.Document.Id}");

                            ProcessCancelledScheduleDocument(firebaseDomain, change);
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Modified)
                        {
                            Console.WriteLine($"Document modified: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListenForCancelledSchedule", $"Document modified: {change.Document.Id}");
                        }
                        else if (change.ChangeType == Google.Cloud.Firestore.DocumentChange.Type.Removed)
                        {
                            Console.WriteLine($"Document removed: {change.Document.Id}");
                            LogManager.Logger.WriteInfo("MyFirestoreClient", "ListePickupBolRetainForChanges", $"Document removed: {change.Document.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MyFirestoreClient:ListenForChanges=> Exception rasied:");
                        Console.WriteLine(ex);
                        LogManager.Logger.WriteException("MyFirestoreClient", "ListenForCancelledSchedule: change.Document.Id=" + change.Document.Id, ex.Message, ex);
                    }
                }
            });
        }
        #endregion
    }
}
