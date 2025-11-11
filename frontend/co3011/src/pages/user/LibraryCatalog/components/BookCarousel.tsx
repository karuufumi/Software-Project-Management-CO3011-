// components/BookCarousel/index.tsx
import { CustomSlider } from "../../../../components/slider";
import Widget from "../../../../components/widget/widget";
import { Link } from "react-router-dom";
import paths from "../../../../routes/paths";

const responsive = {
  desktop: { breakpoint: { max: 3000, min: 1024 }, items: 3 },
  tablet: { breakpoint: { max: 1024, min: 464 }, items: 2 },
  mobile: { breakpoint: { max: 464, min: 0 }, items: 1 },
};

export function BookCarousel({
  title,
  books,
}: {
  title: string;
  books: { bookid: number; title: string; image: string }[];
}) {
  return (
    <>
      <h3 style={{ fontWeight: 600, textTransform: "capitalize", marginBottom: 10 }}>
        {title}
      </h3>

      <CustomSlider
        carouselProps={{
          responsive,
          infinite: true,
          children: books.map((book) => (
            <Widget key={book.bookid} marginLeft={30}>
              <Link
                to={`${paths.USER.LIBRARY_CATALOG}/book/${book.bookid}`}
                style={{ textDecoration: "none", color: "inherit" }}
              >
                <p>{book.title}</p>
                <img src={book.image} alt={book.title} className="logo" />
              </Link>
            </Widget>
          )),
        }}
        style={{ paddingBlock: 10 }}
      />
    </>
  );
}
