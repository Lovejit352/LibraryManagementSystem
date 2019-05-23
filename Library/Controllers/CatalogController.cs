using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Catalog;
using Library.Models.Checkout;
using LibraryData;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _asset;
        private ICheckout _checkout;
        
        public CatalogController(ILibraryAsset asset, ICheckout checkout)
        {
            _asset = asset;
            _checkout = checkout;
        }

        public IActionResult Index()
        {
            var assetModels = _asset.GetAll();

            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _asset.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _asset.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _asset.GetType(result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _asset.GetById(id);

            var currentHolds = _checkout.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel {
                        HoldPlaced = _checkout.GetCurrentHoldPlaced(a.Id).ToString("d"),
                        PatronName = _checkout.GetCurrentHoldPatronName(a.Id)
                });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                AuthorOrDirector = _asset.GetAuthorOrDirector(asset.Id),
                Type = _asset.GetType(asset.Id),
                Year = asset.Year,
                ISBN = _asset.GetIsbn(asset.Id),
                DeweyCallNumber = _asset.GetDeweyIndex(asset.Id),
                Status = asset.Status.Name,
                Cost = asset.Cost,
                CurrentLocation = _asset.GetCurrentLocation(asset.Id).Name,
                ImageUrl = asset.ImageUrl,
                CheckoutHistory = _checkout.GetCheckoutHistory(id),
                LatestCheckout = _checkout.GetLatestCheckout(id),
                PatronName = _checkout.GetCurrentCheckoutPatron(id),
                CurrentHolds = currentHolds
            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _asset.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkout.IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkout.CheckInItem(id);
            return RedirectToAction("Detail", new { id });
        }

        public IActionResult Hold(int id)
        {
            var asset = _asset.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkout.IsCheckedOut(id),
                HoldCount = _checkout.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public IActionResult MarkLost(int assetId)
        {
            _checkout.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkFound(int assetId)
        {
            _checkout.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkout.CheckOutItem(assetId, libraryCardId);

            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkout.PlaceHold(assetId, libraryCardId);

            return RedirectToAction("Detail", new { id = assetId });
        }
    }
}