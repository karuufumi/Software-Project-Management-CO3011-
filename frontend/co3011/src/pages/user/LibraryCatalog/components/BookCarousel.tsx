import { CustomSlider } from "../../../../components/slider";
import Widget from "../../../../components/widget/widget";
import { Link } from "react-router-dom";
import paths from "../../../../routes/paths";

const responsive = {
  desktop: { breakpoint: { max: 3000, min: 1024 }, items: 3 },
  tablet: { breakpoint: { max: 1024, min: 464 }, items: 2 },
  mobile: { breakpoint: { max: 464, min: 0 }, items: 1 },
};

type BookProps = {
  bookId: string;
  title: string;
};

export function BookCarousel({
  title,
  books,
}: {
  title: string;
  books: BookProps[];
}) {
  return (
    <>
      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        {title}
      </h3>

      {books.length === 0 ? (
        <p style={{ opacity: 0.6, marginBottom: 20 }}>
          No books available under this category.
        </p>
      ) : (
        <CustomSlider
          carouselProps={{
            responsive,
            infinite: true,
            children: books.map((book) => (
              <Widget key={book.bookId} marginLeft={30}>
                <Link
                  to={`${paths.USER.LIBRARY_CATALOG}/book/${book.bookId}`}
                  style={{ textDecoration: "none", color: "inherit" }}
                >
                  <div
                    style={{
                      width: 120,
                      height: 160,
                      backgroundColor: "#d9d9d9",
                      borderRadius: 6,
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      padding: 10,
                      fontWeight: 600,
                      marginBottom: 10,
                      textAlign: "center",
                    }}
                  >
                    {book.title}
                  </div>
                  <p style={{ fontSize: 14, fontWeight: 500 }}>{book.title}</p>
                </Link>
              </Widget>
            )),
          }}
          style={{ paddingBlock: 10 }}
        />
      )}
    </>
  );
}
